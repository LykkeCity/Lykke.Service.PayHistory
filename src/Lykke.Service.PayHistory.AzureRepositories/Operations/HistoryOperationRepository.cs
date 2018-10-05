using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.PayHistory.Core.Domain;
using Lykke.Service.PayHistory.Core.Exception;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<IHistoryOperation>> GetByMerchantOrderedByCreatedOnDescAsync(string merchantId)
        {
            return await _storage.GetDataAsync(merchantId, o=> !o.Removed);
        }

        public async Task<IEnumerable<IHistoryOperation>> GetByInvoiceAsync(string invoiceId)
        {
            IEnumerable<AzureIndex> indexes = await _indexByInvoice.GetDataAsync(
                IndexByInvoice.GeneratePartitionKey(invoiceId));

            return await _storage.GetDataAsync(indexes);
        }

        public async Task<IHistoryOperation> GetAsync(string id)
        {
            AzureIndex index =
                await _indexById.GetDataAsync(IndexById.GeneratePartitionKey(id), IndexById.GenerateRowKey());

            return await _storage.GetDataAsync(index);
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

            var tasks = new List<Task>();
            tasks.Add(_storage.InsertOrReplaceAsync(entity));

            AzureIndex indexById = IndexById.Create(entity);
            tasks.Add(_indexById.InsertOrReplaceAsync(indexById));

            if (!string.IsNullOrEmpty(historyOperation.InvoiceId))
            {
                AzureIndex indexByInvoice = IndexByInvoice.Create(entity);
                tasks.Add(_indexByInvoice.InsertOrReplaceAsync(indexByInvoice));
            }

            return Task.WhenAll(tasks);
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
