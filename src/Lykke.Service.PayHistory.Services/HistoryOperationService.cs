using Lykke.Service.PayHistory.Core.Domain;
using Lykke.Service.PayHistory.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.PayHistory.Services
{
    public class HistoryOperationService : IHistoryOperationService
    {
        private readonly IHistoryOperationRepository _historyOperationRepository;

        public HistoryOperationService(IHistoryOperationRepository historyOperationRepository)
        {
            _historyOperationRepository = historyOperationRepository;
        }

        public async Task<IEnumerable<IHistoryOperationView>> GetHistoryAsync(string merchantId)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            return await _historyOperationRepository.GetByMerchantOrderedByCreatedOnDescAsync(
                merchantId);
        }

        public async Task<IEnumerable<IHistoryOperation>> GetHistoryByInvoiceAsync(string invoiceId)
        {
            if (string.IsNullOrWhiteSpace(invoiceId))
            {
                throw new ArgumentNullException(nameof(invoiceId));
            }

            return (await _historyOperationRepository.GetByInvoiceAsync(
                invoiceId)).OrderByDescending(o=>o.CreatedOn);
        }

        public Task<IHistoryOperation> GetDetailsAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _historyOperationRepository.GetAsync(id);
        }

        public Task SetTxHashAsync(string id, string txHash)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _historyOperationRepository.SetTxHashAsync(id, txHash);
        }

        public Task AddAsync(IHistoryOperation historyOperation)
        {
            if (historyOperation == null)
            {
                throw new ArgumentNullException(nameof(historyOperation));
            }

            return _historyOperationRepository.InsertOrReplaceAsync(historyOperation);
        }

        public Task SetRemovedAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _historyOperationRepository.SetRemovedAsync(id);
        }
    }
}
