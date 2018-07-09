using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.PayHistory.Core.Domain;
using System;

namespace Lykke.Service.PayHistory.AzureRepositories.Operations
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class HistoryOperationEntity: AzureTableEntity, IHistoryOperation
    {
        public string Id => RowKey;

        public string MerchantId => PartitionKey;

        private HistoryOperationType _type;
        public HistoryOperationType Type
        {
            get => _type;
            set
            {
                _type = value;
                MarkValueTypePropertyAsDirty(nameof(Type));
            }
        }

        public string OppositeMerchantId
        {
            get;
            set;
        }

        public string InvoiceId
        {
            get;
            set;
        }

        public string InvoiceStatus
        {
            get;
            set;
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get => _createdOn;
            set
            {
                _createdOn = value;
                MarkValueTypePropertyAsDirty(nameof(CreatedOn));
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                MarkValueTypePropertyAsDirty(nameof(Amount));
            }
        }

        public string AssetId
        {
            get;
            set;
        }

        public string DesiredAssetId
        {
            get;
            set;
        }

        public string EmployeeEmail
        {
            get;
            set;
        }

        public string TxHash
        {
            get;
            set;
        }

        private bool? _removed;
        public bool Removed
        {
            get => _removed ?? false;
            set
            {
                _removed = value;
                MarkValueTypePropertyAsDirty(nameof(Removed));
            }
        }

        public HistoryOperationEntity()
        {
        }

        public HistoryOperationEntity(IHistoryOperation historyOperation)
        {
            PartitionKey = GetPartitionKey(historyOperation.MerchantId);
            RowKey = GetRowKey(historyOperation.Id);
            Type = historyOperation.Type;
            OppositeMerchantId = historyOperation.OppositeMerchantId;
            InvoiceId = historyOperation.InvoiceId;
            InvoiceStatus = historyOperation.InvoiceStatus;
            CreatedOn = historyOperation.CreatedOn;
            Amount = historyOperation.Amount;
            AssetId = historyOperation.AssetId;
            DesiredAssetId = historyOperation.DesiredAssetId;
            EmployeeEmail = historyOperation.EmployeeEmail;
            TxHash = historyOperation.TxHash;
            Removed = false;
        }

        internal static string GetPartitionKey(string merchantId)
        {
            if (string.IsNullOrEmpty(merchantId))
            {
                throw new ArgumentNullException(merchantId);
            }

            return merchantId;
        }

        internal static string GetRowKey(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(id);
            }

            return id;
        }
    }
}
