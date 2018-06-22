﻿using System;

namespace Lykke.Service.PayHistory.Core.Domain
{
    public interface IHistoryOperation: IHistoryOperationView
    {
        string MerchantId { get; }

        string InvoiceId { get; }

        string EmployeeEmail { get; }

        string TxHash { get; set; }
    }
}