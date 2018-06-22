using System;

namespace Lykke.Service.PayHistory.Client.Publisher
{
    public interface IHistoryOperation
    {
        string Id { get; }

        HistoryOperationType Type { get; }

        string OppositeMerchantId { get; }

        string Title { get; }

        DateTime CreatedOn { get; }

        decimal Amount { get; }

        string AssetId { get; }
        
        string MerchantId { get; }

        string InvoiceId { get; }

        string EmployeeEmail { get; }

        string TxHash { get; set; }
    }
}
