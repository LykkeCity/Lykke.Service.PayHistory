using Lykke.Service.PayHistory.Core.Domain;
using System;

namespace Lykke.Service.PayHistory.Models
{
    public class HistoryOperationViewModel
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string OppositeMerchantId { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Amount { get; set; }

        public string AssetId { get; set; }
    }
}
