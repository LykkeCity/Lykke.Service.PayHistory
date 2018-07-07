using AzureStorage;
using Lykke.Service.PayHistory.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.PayHistory.Core.Exception;

namespace Lykke.Service.PayHistory.AzureRepositories.Operations
{
    public class HistoryOperationRepository : IHistoryOperationRepository
    {
        private readonly INoSQLTableStorage<HistoryOperationEntity> _storage;

        public HistoryOperationRepository(INoSQLTableStorage<HistoryOperationEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IHistoryOperation>> GetAsync(string merchantId)
        {
            IEnumerable<HistoryOperationEntity> records =
                await _storage.GetDataAsync(HistoryOperationEntity.GetPartitionKey(merchantId));

            return records.Where(x => !x.Removed);
        }

        public async Task<IHistoryOperation> GetAsync(string merchantId, string id)
        {
            return await _storage.GetDataAsync(HistoryOperationEntity.GetPartitionKey(merchantId),
                HistoryOperationEntity.GetRowKey(id));
        }

        public Task SetTxHashAsync(string merchantId, string id, string txHash)
        {
            return _storage.MergeAsync(HistoryOperationEntity.GetPartitionKey(merchantId),
                HistoryOperationEntity.GetRowKey(id),
                o =>
                {
                    o.TxHash = txHash;
                    return o;
                });
        }

        public Task InsertOrReplaceAsync(IHistoryOperation historyOperation)
        {
            return _storage.InsertOrReplaceAsync(new HistoryOperationEntity(historyOperation));
        }

        public async Task SetRemovedAsync(string merchantId, string id)
        {
            HistoryOperationEntity updated = await _storage.MergeAsync(
                HistoryOperationEntity.GetPartitionKey(merchantId),
                HistoryOperationEntity.GetRowKey(id),
                o =>
                {
                    o.Removed = true;
                    return o;
                });

            if (updated == null)
                throw new HistoryOperationNotFoundException(merchantId, id);
        }
    }
}
