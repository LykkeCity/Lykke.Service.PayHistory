using AzureStorage;
using Lykke.Service.PayHistory.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage.Tables.Templates.Index;

namespace Lykke.Service.PayHistory.AzureRepositories.Operations
{
    public class HistoryOperationRepository : IHistoryOperationRepository
    {
        private readonly INoSQLTableStorage<HistoryOperationEntity> _storage;
        private readonly INoSQLTableStorage<AzureIndex> _indexByInvoice;

        public HistoryOperationRepository(INoSQLTableStorage<HistoryOperationEntity> storage,
            INoSQLTableStorage<AzureIndex> indexByInvoice)
        {
            _storage = storage;
            _indexByInvoice = indexByInvoice;
        }

        public async Task<IEnumerable<IHistoryOperation>> GetAsync(string merchantId)
        {
            return await _storage.GetDataAsync(HistoryOperationEntity.GetPartitionKey(merchantId));
        }

        public async Task<IEnumerable<IHistoryOperation>> GetByInvoiceAsync(string invoiceId)
        {
            IEnumerable<AzureIndex> indexes =
                await _indexByInvoice.GetDataAsync(IndexByInvoice.GeneratePartitionKey(invoiceId));

            return await _storage.GetDataAsync(indexes);
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
            var entity = new HistoryOperationEntity(historyOperation);

            if (string.IsNullOrEmpty(historyOperation.InvoiceId))
            {
                return _storage.InsertOrReplaceAsync(entity);
            }
            else
            {
                AzureIndex index = IndexByInvoice.Create(entity);

                return Task.WhenAll(_storage.InsertOrReplaceAsync(entity),
                    _indexByInvoice.InsertOrReplaceAsync(index));
            }
        }
    }
}
