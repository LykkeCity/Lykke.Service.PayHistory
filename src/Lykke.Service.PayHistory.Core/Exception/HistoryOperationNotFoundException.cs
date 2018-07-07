using System.Runtime.Serialization;

namespace Lykke.Service.PayHistory.Core.Exception
{
    public class HistoryOperationNotFoundException : System.Exception
    {
        public HistoryOperationNotFoundException()
        {
        }

        public HistoryOperationNotFoundException(string merchantId, string operationId) : base("History operation not found")
        {
            MerchantId = merchantId;
            OperationId = operationId;
        }

        public HistoryOperationNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected HistoryOperationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string MerchantId { get; set; }
        public string OperationId { get; set; }
    }
}
