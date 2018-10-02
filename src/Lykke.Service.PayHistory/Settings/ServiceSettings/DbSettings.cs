using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayHistory.Settings.ServiceSettings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [AzureTableCheck]
        public string DataConnString { get; set; }

        public string OperationsTableName { get; set; }

        public string OrderedOperationsTableName { get; set; }
    }
}
