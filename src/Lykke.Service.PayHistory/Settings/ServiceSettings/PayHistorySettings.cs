namespace Lykke.Service.PayHistory.Settings.ServiceSettings
{
    public class PayHistorySettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSubscriberSettings Rabbit { get; set; }
    }
}
