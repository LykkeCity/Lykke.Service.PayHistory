using System;
using Lykke.Service.PayHistory.AutorestClient.Models;

namespace Lykke.Service.PayHistory.Client.Publisher
{
    public interface IHistoryOperation
    {
        string Id { get; }

        HistoryOperationType Type { get; }

        string OppositeMerchantId { get; }

        DateTime CreatedOn { get; }

        decimal Amount { get; }

        string AssetId { get; }

        string DesiredAssetId { get; }

        string InvoiceId { get; }

        string InvoiceStatus { get; }

        string MerchantId { get; }

        string EmployeeEmail { get; }

        string TxHash { get; set; }
    }
}
