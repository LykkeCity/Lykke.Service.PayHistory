using Lykke.Service.PayHistory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.PayHistory.Core.Services;

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

            return await _historyOperationRepository.GetAsync(merchantId);
        }

        public Task<IHistoryOperation> GetDetailsAsync(string merchantId, string id)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _historyOperationRepository.GetAsync(merchantId, id);
        }

        public Task SetTxHashAsync(string merchantId, string id, string txHash)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _historyOperationRepository.SetTxHashAsync(merchantId, id, txHash);
        }

        public Task AddAsync(IHistoryOperation historyOperation)
        {
            if (historyOperation == null)
            {
                throw new ArgumentNullException(nameof(historyOperation));
            }

            return _historyOperationRepository.InsertOrReplaceAsync(historyOperation);
        }
    }
}
