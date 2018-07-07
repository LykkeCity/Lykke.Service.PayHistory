using Lykke.Service.PayHistory.Core.Domain;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.PayHistory.Models
{
    public class HistoryOperationModel : HistoryOperationViewModel
    {  
        public string MerchantId { get; set; }

        public string EmployeeEmail { get; set; }

        public string TxHash { get; set; }
    }
}
