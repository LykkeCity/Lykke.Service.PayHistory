using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PayHistory.Core.Domain;
using Lykke.Service.PayHistory.Settings.ServiceSettings;
using System;
using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.Service.PayHistory.Core.Services;

namespace Lykke.Service.PayHistory.Rabbit
{
    public class HistoryOperationSubscruber : IStartable, IStopable
    {
        private RabbitMqSubscriber<HistoryOperation> _subscriber;
        private readonly RabbitMqSubscriberSettings _settings;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;
        private readonly IHistoryOperationService _historyOperationService;

        public HistoryOperationSubscruber(IHistoryOperationService historyOperationService,
            RabbitMqSubscriberSettings settings, ILogFactory logFactory)
        {
            _historyOperationService = historyOperationService;
            _settings = settings;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
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

            var errorHandlingStrategy = new ResilientErrorHandlingStrategy(_logFactory, 
                settings,
                retryTimeout: TimeSpan.FromSeconds(10),
                next: new DeadQueueErrorHandlingStrategy(_logFactory, settings));

            _subscriber = new RabbitMqSubscriber<HistoryOperation>(_logFactory, settings,
                    errorHandlingStrategy)
                .SetMessageDeserializer(new JsonMessageDeserializer<HistoryOperation>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();

            _log.Info($"<< {nameof(HistoryOperationSubscruber)} is started.");
        }

        private Task ProcessMessageAsync(HistoryOperation historyOperation)
        {
            return _historyOperationService.AddAsync(historyOperation);
        }

        public void Stop()
        {
            _subscriber?.Stop();

            _log.Info($"<< {nameof(HistoryOperationSubscruber)} is stopped.");
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }
    }
}
