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

        private string _oppositeMerchantId;
        public string OppositeMerchantId
        {
            get => _oppositeMerchantId;
            set
            {
                _oppositeMerchantId = value;
                MarkValueTypePropertyAsDirty(nameof(OppositeMerchantId));
            }
        }

        private string _invoiceId;
        public string InvoiceId
        {
            get => _invoiceId;
            set
            {
                _invoiceId = value;
                MarkValueTypePropertyAsDirty(nameof(InvoiceId));
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                MarkValueTypePropertyAsDirty(nameof(Title));
            }
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

        private string _assetId;
        public string AssetId
        {
            get => _assetId;
            set
            {
                _assetId = value;
                MarkValueTypePropertyAsDirty(nameof(AssetId));
            }
        }

        private string _employeeEmail;
        public string EmployeeEmail
        {
            get => _employeeEmail;
            set
            {
                _employeeEmail = value;
                MarkValueTypePropertyAsDirty(nameof(EmployeeEmail));
            }
        }

        private string _txHash;
        public string TxHash
        {
            get => _txHash;
            set
            {
                _txHash = value;
                MarkValueTypePropertyAsDirty(nameof(TxHash));
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
            Title = historyOperation.Title;
            CreatedOn = historyOperation.CreatedOn;
            Amount = historyOperation.Amount;
            AssetId = historyOperation.AssetId;
            EmployeeEmail = historyOperation.EmployeeEmail;
            TxHash = historyOperation.TxHash;
        }

        internal static string GetPartitionKey(string merchantId)
            => merchantId;
        internal static string GetRowKey(string id)
            => id;
    }
}
