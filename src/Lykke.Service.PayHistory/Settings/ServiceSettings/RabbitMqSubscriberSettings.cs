using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayHistory.Settings.ServiceSettings
{
    public class RabbitMqSubscriberSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string ExchangeName { get; set; }

        public string QueueName { get; set; }
    }
}
