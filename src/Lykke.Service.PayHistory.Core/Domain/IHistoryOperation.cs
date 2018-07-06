namespace Lykke.Service.PayHistory.Core.Domain
{
    public interface IHistoryOperation: IHistoryOperationView
    {        
        string MerchantId { get; }        

        string EmployeeEmail { get; }

        string TxHash { get; set; }
    }
}
