﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.PayHistory.Client.AutorestClient;
using Lykke.Service.PayHistory.Client.AutorestClient.Models;
using Lykke.Service.PayHistory.Client.Models;

namespace Lykke.Service.PayHistory.Client
{
    public class PayHistoryClient : IPayHistoryClient, IDisposable
    {
        private readonly ILog _log;
        private readonly IPayHistoryAPI _service;
        
        public PayHistoryClient(string serviceUrl, ILog log)
        {
            _log = log;
            _service = new PayHistoryAPI(new Uri(serviceUrl));
        }

        #region IsAliveController
        /// <summary>
        /// Checks service is alive
        /// </summary>
        public IsAliveResponse IsAlive()
        {
            object result = _service.IsAlive();
            return Convert<IsAliveResponse>(result);
        }

        /// <summary>
        /// Checks service is alive
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        public async Task<IsAliveResponse> IsAliveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            object result = await _service.IsAliveAsync(cancellationToken);
            return Convert<IsAliveResponse>(result);
        }
        #endregion IsAliveController

        #region HistoryOperationController
        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <returns>History operations.</returns>
        public IEnumerable<HistoryOperationViewModel> GetHistory(string merchantId)
        {
            var result = _service.GetHistory(merchantId);
            return Convert<IEnumerable<HistoryOperationViewModel>>(result);
        }

        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>History operations.</returns>
        public async Task<IEnumerable<HistoryOperationViewModel>> GetHistoryAsync(string merchantId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _service.GetHistoryAsync(merchantId, cancellationToken);
            return Convert<IEnumerable<HistoryOperationViewModel>>(result);
        }

        /// <summary>
        /// Returns details of the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <returns>Details of the history operation.</returns>
        public HistoryOperationModel GetDetails(string merchantId, string id)
        {
            var result = _service.GetDetails(merchantId, id);
            return Convert<HistoryOperationModel>(result);
        }

        /// <summary>
        /// Returns details of the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>Details of the history operation.</returns>
        public async Task<HistoryOperationModel> GetDetailsAsync(string merchantId, string id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _service.GetDetailsAsync(merchantId, id, cancellationToken);
            return Convert<HistoryOperationModel>(result);
        }

        /// <summary>
        /// Set TxHash to the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="txHash">TxHash of the history operation.</param>
        public void SetTxHash(string merchantId, string id, string txHash)
        {
            var result = _service.SetTxHash(merchantId, id, txHash);
            Convert<object>(result);
        }

        /// <summary>
        /// Set TxHash to the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="txHash">TxHash of the history operation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        public async Task SetTxHashAsync(string merchantId, string id, string txHash,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _service.SetTxHashAsync(merchantId, id, txHash, cancellationToken);
            Convert<object>(result);
        }

        #endregion HistoryOperationController

        public void Dispose()
        {
            _service?.Dispose();
        }

        private static T Convert<T>(object result) where T : class
        {
            if (result is ErrorResponse errorResponse)
            {
                throw new PayHistoryApiException(errorResponse);
            }

            return result as T;
        }
    }
}