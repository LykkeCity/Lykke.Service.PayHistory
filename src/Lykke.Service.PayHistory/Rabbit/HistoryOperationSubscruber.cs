using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PayHistory.Core.Domain;
using Lykke.Service.PayHistory.Settings.ServiceSettings;
using System;
using System.Threading.Tasks;
using Lykke.Service.PayHistory.Core.Services;

namespace Lykke.Service.PayHistory.Rabbit
{
    public class HistoryOperationSubscruber : IStartable, IStopable
    {
        private RabbitMqSubscriber<HistoryOperation> _subscriber;
        private readonly RabbitMqSubscriberSettings _settings;
        private readonly ILog _log;
        private readonly IHistoryOperationService _historyOperationService;

        public HistoryOperationSubscruber(IHistoryOperationService historyOperationService,
            RabbitMqSubscriberSettings settings, ILog log)
        {
            _historyOperationService = historyOperationService;
            _settings = settings;
            _log = log;
        }

        public void Start()
        {
            var settings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = _settings.ConnectionString,
                ExchangeName = _settings.ExchangeName,
                QueueName = _settings.QueueName,
                IsDurable = true
            };

            var errorHandlingStrategy = new ResilientErrorHandlingStrategy(_log, settings,
                retryTimeout: TimeSpan.FromSeconds(10),
                next: new DeadQueueErrorHandlingStrategy(_log, settings));

            _subscriber = new RabbitMqSubscriber<HistoryOperation>(settings,
                    errorHandlingStrategy)
                .SetMessageDeserializer(new JsonMessageDeserializer<HistoryOperation>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetLogger(_log)
                .Start();

            _log.WriteInfo(nameof(HistoryOperationSubscruber),
                nameof(Start),
                $"<< {nameof(HistoryOperationSubscruber)} is started.");
        }

        private Task ProcessMessageAsync(HistoryOperation historyOperation)
        {
            return _historyOperationService.AddAsync(historyOperation);
        }

        public void Stop()
        {
            _subscriber?.Stop();

            _log.WriteInfo(nameof(HistoryOperationSubscruber),
                nameof(Stop),
                $"<< {nameof(HistoryOperationSubscruber)} is stopped.");
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }
    }
}
