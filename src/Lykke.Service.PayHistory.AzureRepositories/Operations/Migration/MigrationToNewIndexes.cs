using Autofac;
using AzureStorage;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.PayHistory.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.PayHistory.AzureRepositories.Operations.Migration
{
    /// <summary>
    /// Temporary class for migration from previous version
    /// </summary>
    public class MigrationToNewIndexes: IStartable, IDisposable
    {
        private readonly INoSQLTableStorage<OldHistoryOperationEntity> _oldStorage;
        private readonly IHistoryOperationRepository _repository;
        private Timer _timer;
        private readonly ILog _log;

        public MigrationToNewIndexes(
            INoSQLTableStorage<OldHistoryOperationEntity> oldStorage,
            IHistoryOperationRepository repository,
            ILogFactory logFactory)
        {
            _oldStorage = oldStorage;
            _repository = repository;
            _log = logFactory.CreateLog(this);
        }

        private async Task ProcessAsync()
        {
            var oldEntity = await _oldStorage.GetTopRecordAsync(new TableQuery<OldHistoryOperationEntity>());
            if (oldEntity == null)
            {
                return;
            }

            await AddNewAsync();
            await RemoveOldIndexesAsync();
        }

        private async Task AddNewAsync()
        {
            _log.Info("Add new indexes >>");

            string continuationToken = null;
            
            do
            {
                var result = await _oldStorage.GetDataWithContinuationTokenAsync(300, continuationToken);
                continuationToken = result.ContinuationToken;

                var tasks = new List<Task>();
                foreach (var entity in result.Entities)
                {
                    tasks.Add(AddNewAsync(entity));
                }

                await Task.WhenAll(tasks);
            } while (continuationToken != null);

            _log.Info("Add new indexes <<");
        }

        private async Task AddNewAsync(OldHistoryOperationEntity entity)
        {
            if (entity.CreatedOn == DateTime.MinValue)
            {
                return;
            }

            await _repository.InsertOrReplaceAsync(entity);

            if (entity.Removed)
            {
                await _repository.SetRemovedAsync(entity.Id);
            }
        }

        private async Task RemoveOldIndexesAsync()
        {
            _log.Info("Remove old indexes >>");

            int retry = 3;
            while (true)
            {
                try
                {
                    var result = await _oldStorage.GetDataWithContinuationTokenAsync(100, null);

                    var tasks = new List<Task>();
                    foreach (var entity in result.Entities)
                    {
                        tasks.Add(_oldStorage.DeleteAsync(entity));
                    }
                    await Task.WhenAll(tasks);

                    if (result.ContinuationToken == null)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    retry--;
                    if (retry <= 0)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }

            _log.Info("Remove old indexes <<");
        }

        public void Start()
        {
            _timer = new Timer(
                async (s) => await ProcessAsync(),
                null,
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
