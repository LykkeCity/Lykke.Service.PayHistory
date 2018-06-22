using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using System.Threading.Tasks;
using Lykke.Service.PayHistory.Client.Models;

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
            Validate(historyOperation);

            await Task.WhenAll(_publisher.ProduceAsync(historyOperation),
                _log.WriteInfoAsync(nameof(HistoryOperationPublisher), nameof(PublishAsync),
                    historyOperation.ToJson()));
        }


        protected virtual void Validate(IHistoryOperation historyOperation)
        {
            var context = new ValidationContext(historyOperation);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(historyOperation, context, results, true);

            if (!isValid)
            {
                var modelErrors = new Dictionary<string, IList<string>>();
                foreach (ValidationResult validationResult in results)
                {
                    foreach (string memberName in validationResult.MemberNames)
                    {
                        if (!modelErrors.TryGetValue(memberName, out var errors))
                        {
                            errors = new List<string>();
                            modelErrors[memberName] = errors;
                        }

                        errors.Add(validationResult.ErrorMessage);
                    }
                }

                throw new PayHistoryApiException("Model is invalid.", modelErrors);
            }
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
