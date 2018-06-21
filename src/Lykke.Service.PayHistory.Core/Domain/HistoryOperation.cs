using System;

namespace Lykke.Service.PayHistory.Core.Domain
{
    public class HistoryOperation: IHistoryOperation
    {
        public string Id { get; set; }

        public HistoryOperationType Type { get; set; }

        public string OppositeMerchantId { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Amount { get; set; }

        public string AssetId { get; set; }

        public string MerchantId { get; set; }

        public string InvoiceId { get; set; }

        public string EmployeeEmail { get; set; }

        public string TxHash { get; set; }
    }
}
