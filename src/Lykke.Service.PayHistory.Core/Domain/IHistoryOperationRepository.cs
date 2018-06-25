﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PayHistory.Core.Domain
{
    public interface IHistoryOperationRepository
    {
        Task<IEnumerable<IHistoryOperation>> GetAsync(string merchantId);
        Task<IHistoryOperation> GetAsync(string merchantId, string id);
        Task SetTxHashAsync(string merchantId, string id, string txHash);
        Task InsertOrReplaceAsync(IHistoryOperation historyOperation);
    }
}
