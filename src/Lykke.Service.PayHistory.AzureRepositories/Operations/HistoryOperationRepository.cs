using AzureStorage;
using Lykke.Service.PayHistory.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.PayHistory.Core.Exception;

namespace Lykke.Service.PayHistory.AzureRepositories.Operations
{
    public class HistoryOperationRepository : IHistoryOperationRepository
    {
        private readonly INoSQLTableStorage<HistoryOperationEntity> _storage;
        private readonly INoSQLTableStorage<AzureIndex> _indexByInvoice;
        private readonly INoSQLTableStorage<AzureIndex> _indexById;

        public HistoryOperationRepository(
            INoSQLTableStorage<HistoryOperationEntity> storage,
            INoSQLTableStorage<AzureIndex> indexByInvoice, 
            INoSQLTableStorage<AzureIndex> indexById)
        {
            _storage = storage;
            _indexByInvoice = indexByInvoice;
            _indexById = indexById;
        }

        public async Task<IEnumerable<IHistoryOperation>> GetAsync(string merchantId)
        {
            IEnumerable<HistoryOperationEntity> records =
                await _storage.GetDataAsync(HistoryOperationEntity.GetPartitionKey(merchantId));

            return records.Where(x => !x.Removed);
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

        public async Task SetTxHashAsync(string id, string txHash)
        {
            AzureIndex index =
                await _indexById.GetDataAsync(IndexById.GeneratePartitionKey(id), IndexById.GenerateRowKey());

            if (index == null)
                throw new HistoryOperationNotFoundException(id);

            HistoryOperationEntity updated = await _storage.MergeAsync(
                index.PrimaryPartitionKey, index.PrimaryRowKey,
                o =>
                {
                    o.TxHash = txHash;
                    return o;
                });

            if (updated == null)
                throw new HistoryOperationNotFoundException(id);
        }

        public Task InsertOrReplaceAsync(IHistoryOperation historyOperation)
        {
            var entity = new HistoryOperationEntity(historyOperation);

            AzureIndex indexById = IndexById.Create(entity);

            if (string.IsNullOrEmpty(historyOperation.InvoiceId))
            {
                return Task.WhenAll(
                    _storage.InsertOrReplaceAsync(entity),
                    _indexById.InsertOrReplaceAsync(indexById));
            }

            AzureIndex indexByInvoice = IndexByInvoice.Create(entity);

            return Task.WhenAll(
                _storage.InsertOrReplaceAsync(entity),
                _indexById.InsertOrReplaceAsync(indexById),
                _indexByInvoice.InsertOrReplaceAsync(indexByInvoice));
        }

        public async Task SetRemovedAsync(string id)
        {
            AzureIndex index =
                await _indexById.GetDataAsync(IndexById.GeneratePartitionKey(id), IndexById.GenerateRowKey());

            if (index == null)
                throw new HistoryOperationNotFoundException(id);

            HistoryOperationEntity updated = await _storage.MergeAsync(
                index.PrimaryPartitionKey,
                index.PrimaryRowKey,
                o =>
                {
                    o.Removed = true;
                    return o;
                });

            if (updated == null)
                throw new HistoryOperationNotFoundException(id);
        }
    }
}
