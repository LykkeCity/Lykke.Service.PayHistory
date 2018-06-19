using System;
using Lykke.Sdk;
using Lykke.Service.PayHistory.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.PayHistory
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.ApiTitle = "PayHistory API";
                options.Logs = ("PayHistoryLog", ctx => ctx.PayHistoryService.Db.LogsConnString);
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration();

        }
    }
}
