using System;
using Autofac;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.PayHistory.Client.Publisher;

namespace Lykke.Service.PayHistory.Client
{
    public static class AutofacExtension
    {
        public static void RegisterPayHistoryClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));

            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<PayHistoryClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IPayHistoryClient>()
                .SingleInstance();
        }

        public static void RegisterHistoryOperationPublisher(this ContainerBuilder builder,
            RabbitMqPublisherSettings settings)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            builder.RegisterType<HistoryOperationPublisher>()
                .AsSelf()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .UsingConstructor(typeof(RabbitMqPublisherSettings), typeof(ILogFactory))
                .WithParameter("settings", settings);
        }

        [Obsolete]
        public static void RegisterHistoryOperationPublisher(this ContainerBuilder builder,
            RabbitMqPublisherSettings settings, ILog log)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            builder.RegisterType<HistoryOperationPublisher>()
                .AsSelf()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .UsingConstructor(typeof(RabbitMqPublisherSettings), typeof(ILog))
                .WithParameter("settings", settings)
                .WithParameter("log", log);
        }
    }
}
