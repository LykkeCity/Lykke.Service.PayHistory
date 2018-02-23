using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayHistory.Settings.ServiceSettings
{
    public class RabbitMqSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string PaymentRequestsExchangeName { get; set; }

        public string TransferRequestsExchangeName { get; set; }
    }
}
