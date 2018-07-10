using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.Service.PayHistory.Client.Models;

namespace Lykke.Service.PayHistory.Client.Publisher
{
    public class HistoryOperationPublisher : IStartable, IStopable
    {
        private readonly RabbitMqPublisherSettings _settings;
        private readonly ILog _log;
        private readonly ILogFactory _logFactory;
        private RabbitMqPublisher<IHistoryOperation> _publisher;

        [Obsolete]
        public HistoryOperationPublisher(RabbitMqPublisherSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        public HistoryOperationPublisher(RabbitMqPublisherSettings settings, 
            ILogFactory logFactory)
        {
            _settings = settings;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings.CreateForPublisher(
                _settings.ConnectionString, _settings.ExchangeName);
            settings.MakeDurable();

            if (_logFactory == null)
            {
                _publisher = new RabbitMqPublisher<IHistoryOperation>(settings)
                    .SetConsole(new LogToConsole())
                    .SetLogger(_log);
            }
            else
            {
                _publisher = new RabbitMqPublisher<IHistoryOperation>(_logFactory, settings);
            }

            _publisher.DisableInMemoryQueuePersistence()
                .PublishSynchronously()
                .SetSerializer(new JsonMessageSerializer<IHistoryOperation>())
                .SetPublishStrategy(new DefaultFanoutPublishStrategy(settings))
                .Start();
        }

        public async Task PublishAsync(IHistoryOperation historyOperation)
        {
            Validate(historyOperation);

            await _publisher.ProduceAsync(historyOperation);

            _log.Info("Pay history operation is published.", historyOperation.ToJson());
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
                    if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                    {
                        foreach (string memberName in validationResult.MemberNames)
                        {
                            AddModelError(modelErrors, memberName, validationResult.ErrorMessage);
                        }
                    }
                    else
                    {
                        AddModelError(modelErrors, string.Empty, validationResult.ErrorMessage);
                    }
                }

                throw new PayHistoryApiException("Model is invalid.", modelErrors);
            }
        }

        private void AddModelError(Dictionary<string, IList<string>> modelErrors, string memberName, 
            string errorMessage)
        {
            if (!modelErrors.TryGetValue(memberName, out var errors))
            {
                errors = new List<string>();
                modelErrors[memberName] = errors;
            }

            errors.Add(errorMessage);
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
