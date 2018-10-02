using System;
using AzureStorage.Tables.Templates.Index;

namespace Lykke.Service.PayHistory.AzureRepositories.Operations
{
    internal static class IndexById
    {
        internal static string GeneratePartitionKey(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(id);
            }

            return id;
        }

        internal static string GenerateRowKey()
        {
            return "IndexById";
        }

        internal static AzureIndex Create(HistoryOperationEntity entity)
        {
            return AzureIndex.Create(GeneratePartitionKey(entity.Id), GenerateRowKey(), entity);
        }
    }
}
