using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PayHistory.Core.Domain
{
    public interface IHistoryOperationRepository
    {
        Task<IEnumerable<IHistoryOperation>> GetAsync(string merchantId);
        Task<IEnumerable<IHistoryOperation>> GetByInvoiceAsync(string invoiceId);
        Task<IHistoryOperation> GetAsync(string merchantId, string id);
        Task SetTxHashAsync(string merchantId, string id, string txHash);
        Task InsertOrReplaceAsync(IHistoryOperation historyOperation);
        Task SetRemovedAsync(string merchantId, string id);
    }
}
