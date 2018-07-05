using System;

namespace Lykke.Service.PayHistory.Core.Domain
{
    public interface IHistoryOperationView
    {
        string Id { get; }

        HistoryOperationType Type { get; }

        string OppositeMerchantId { get; }

        DateTime CreatedOn { get; }

        decimal Amount { get; }

        string AssetId { get; }

        string DesiredAssetId { get; }

        string InvoiceId { get; }
    }
}
