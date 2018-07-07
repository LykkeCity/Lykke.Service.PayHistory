using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Lykke.Service.PayHistory.AutorestClient.Models;

namespace Lykke.Service.PayHistory.Client.Models
{
    [Serializable]
    public class PayHistoryApiException : Exception
    {
        public IDictionary<string, IList<string>> ModelErrors { get; }

        public PayHistoryApiException()
        {
        }

        public PayHistoryApiException(ErrorResponse errorResponse)
            : base(errorResponse.ErrorMessage)
        {
            ModelErrors = errorResponse.ModelErrors;
        }

        public PayHistoryApiException(string message,
            IDictionary<string, IList<string>> modelErrors = null)
            : base(message)
        {
            ModelErrors = modelErrors;
        }

        public PayHistoryApiException(string message,
            Exception innerException,
            IDictionary<string, IList<string>> modelErrors = null)
            : base(message, innerException)
        {
            ModelErrors = modelErrors;
        }

        protected PayHistoryApiException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
