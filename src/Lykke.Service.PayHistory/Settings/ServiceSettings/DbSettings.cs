using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayHistory.Settings.ServiceSettings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
