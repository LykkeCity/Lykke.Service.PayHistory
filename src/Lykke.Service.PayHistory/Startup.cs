﻿using Lykke.Sdk;
using Lykke.Service.PayHistory.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lykke.Service.PayHistory
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var result = services.BuildServiceProvider<AppSettings>(options =>
            {
                options.ApiTitle = "PayHistory API";
                options.Logs = ("PayHistoryLog", ctx => ctx.PayHistoryService.Db.LogsConnString);
            });

            return result;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration();
        }
    }
}
