using AzureStorage;
using Lykke.Service.PayHistory.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _storage.GetDataAsync(HistoryOperationEntity.GetPartitionKey(merchantId));
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
    }
}
