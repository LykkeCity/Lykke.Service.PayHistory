namespace Lykke.Service.PayHistory.Core.Domain
{
    public interface IHistoryOperation: IHistoryOperationView
    {
        string InvoiceStatus { get; set; }

        string MerchantId { get; }        

        string EmployeeEmail { get; }

        string TxHash { get; set; }
    }
}
