using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.PayHistory.Client.Publisher
{
    public class HistoryOperation : IHistoryOperation
    {
        public string Id { get; }

        public HistoryOperation()
        {
            Id = Guid.NewGuid().ToString();
        }

        public HistoryOperation(string id)
        {
            Id = id;
        }

        [JsonConverter(typeof(StringEnumConverter))]
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
