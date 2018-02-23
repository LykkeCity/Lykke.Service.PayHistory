using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.PayHistory.Client
{
    public static class AutofacExtension
    {
        public static void RegisterPayHistoryClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<PayHistoryClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IPayHistoryClient>()
                .SingleInstance();
        }

        public static void RegisterPayHistoryClient(this ContainerBuilder builder, PayHistoryServiceClientSettings settings, ILog log)
        {
            builder.RegisterPayHistoryClient(settings?.ServiceUrl, log);
        }
    }
}
