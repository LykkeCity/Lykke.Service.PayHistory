using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.PayHistory.Settings.ServiceSettings;

namespace Lykke.Service.PayHistory.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public PayHistorySettings PayHistoryService { get; set; }
    }
}
