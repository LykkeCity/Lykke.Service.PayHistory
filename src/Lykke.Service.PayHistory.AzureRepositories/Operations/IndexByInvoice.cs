using System;
using AzureStorage.Tables.Templates.Index;

namespace Lykke.Service.PayHistory.AzureRepositories.Operations
{
    internal static class IndexByInvoice
    {
        internal static string GeneratePartitionKey(string invoiceId)
        {
            if (string.IsNullOrEmpty(invoiceId))
            {
                throw new ArgumentNullException(invoiceId);
            }

            return invoiceId;
        }

        internal static string GenerateRowKey(DateTime createdOn, string id)
        {
            return $"{(DateTime.MaxValue.Ticks - createdOn.Ticks):D19}{id}";
        }

        internal static AzureIndex Create(HistoryOperationEntity entity)
        {
            return AzureIndex.Create(GeneratePartitionKey(entity.InvoiceId), 
                GenerateRowKey(entity.CreatedOn, entity.Id), entity);
        }
    }
}
