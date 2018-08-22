using Autofac;
using AutoMapper;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common;
using Lykke.Common.Log;
using Lykke.Service.PayHistory.AzureRepositories.Operations;
using Lykke.Service.PayHistory.Core.Domain;
using Lykke.Service.PayHistory.Core.Services;
using Lykke.Service.PayHistory.Rabbit;
using Lykke.Service.PayHistory.Services;
using Lykke.Service.PayHistory.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.PayHistory.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Do not register entire settings in container, pass necessary settings to services which requires them

            var mapperProvider = new MapperProvider();
            IMapper mapper = mapperProvider.GetMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            builder.Register(c =>
                new HistoryOperationRepository(
                    AzureTableStorage<HistoryOperationEntity>.Create(
                        _appSettings.ConnectionString(x => x.PayHistoryService.Db.DataConnString),
                        _appSettings.CurrentValue.PayHistoryService.Db.OperationsTableName,
                        c.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(
                        _appSettings.ConnectionString(x => x.PayHistoryService.Db.DataConnString),
                        _appSettings.CurrentValue.PayHistoryService.Db.OperationsTableName,
                        c.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(
                        _appSettings.ConnectionString(x => x.PayHistoryService.Db.DataConnString),
                        _appSettings.CurrentValue.PayHistoryService.Db.OperationsTableName,
                        c.Resolve<ILogFactory>())))
                .As<IHistoryOperationRepository>()
                .SingleInstance();

            builder.RegisterType<HistoryOperationService>()
                .As<IHistoryOperationService>()
                .SingleInstance();

            builder.RegisterType<HistoryOperationSubscruber>()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter("settings", _appSettings.CurrentValue.PayHistoryService.Rabbit);
        }
    }
}
