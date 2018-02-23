using Lykke.Service.PayHistory.Settings.ServiceSettings;
using Lykke.Service.PayHistory.Settings.SlackNotifications;

namespace Lykke.Service.PayHistory.Settings
{
    public class AppSettings
    {
        public PayHistorySettings PayHistoryService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
