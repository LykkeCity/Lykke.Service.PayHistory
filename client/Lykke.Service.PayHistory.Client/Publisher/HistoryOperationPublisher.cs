﻿using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using System.Threading.Tasks;

namespace Lykke.Service.PayHistory.Client.Publisher
{
    public class HistoryOperationPublisher : IStartable, IStopable
    {
        private readonly RabbitMqPublisherSettings _settings;
        private readonly ILog _log;
        private RabbitMqPublisher<IHistoryOperation> _publisher;

        public HistoryOperationPublisher(RabbitMqPublisherSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings.CreateForPublisher(_settings.ConnectionString, _settings.ExchangeName);
            settings.MakeDurable();

            _publisher = new RabbitMqPublisher<IHistoryOperation>(settings)
                .DisableInMemoryQueuePersistence()
                .SetSerializer(new JsonMessageSerializer<IHistoryOperation>())
                .SetPublishStrategy(new DefaultFanoutPublishStrategy(settings))
                .SetConsole(new LogToConsole())
                .SetLogger(_log)
                .Start();
        }

        public async Task PublishAsync(IHistoryOperation historyOperation)
        {
            await Task.WhenAll(_publisher.ProduceAsync(historyOperation),
                _log.WriteInfoAsync(nameof(HistoryOperationPublisher), nameof(PublishAsync),
                    historyOperation.ToJson()));
        }

        public void Dispose()
        {
            _publisher?.Dispose();
        }

        public void Stop()
        {
            _publisher?.Stop();
        }
    }
}
