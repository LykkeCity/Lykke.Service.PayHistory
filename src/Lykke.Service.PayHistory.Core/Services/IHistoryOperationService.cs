using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.PayHistory.Core.Domain;

namespace Lykke.Service.PayHistory.Core.Services
{
    public interface IHistoryOperationService
    {
        Task<IEnumerable<IHistoryOperationView>> GetHistoryAsync(string merchantId);
        Task<IEnumerable<IHistoryOperation>> GetHistoryByInvoiceAsync(string invoiceId);
        Task<IHistoryOperation> GetDetailsAsync(string merchantId, string id);
        Task SetTxHashAsync(string merchantId, string id, string txHash);
        Task AddAsync(IHistoryOperation historyOperation);
    }
}
