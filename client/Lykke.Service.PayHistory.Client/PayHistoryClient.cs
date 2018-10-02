using Lykke.Service.PayHistory.Client.AutorestClient;
using Lykke.Service.PayHistory.Client.AutorestClient.Models;
using Lykke.Service.PayHistory.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.PayHistory.Client
{
    public class PayHistoryClient : IPayHistoryClient, IDisposable
    {
        private readonly IPayHistoryAPI _service;
        
        public PayHistoryClient(string serviceUrl)
        {
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
        /// Returns history operations base info.
        /// </summary>
        /// <param name="invoiceId">Identifier of the invoice.</param>
        /// <returns>History operations.</returns>
        public IEnumerable<HistoryOperationViewModel> GetHistoryByInvoice(string invoiceId)
        {
            var result = _service.GetHistoryByInvoice(invoiceId);
            return Convert<IEnumerable<HistoryOperationViewModel>>(result);
        }

        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="invoiceId">Identifier of the invoice.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>History operations.</returns>
        public async Task<IEnumerable<HistoryOperationViewModel>> GetHistoryByInvoiceAsync(string invoiceId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _service.GetHistoryByInvoiceAsync(invoiceId, cancellationToken);
            return Convert<IEnumerable<HistoryOperationViewModel>>(result);
        }

        /// <summary>
        /// Returns details of the history operation.
        /// </summary>
        /// <param name="id">Identifier of the history operation.</param>
        /// <returns>Details of the history operation.</returns>
        public HistoryOperationModel GetDetails(string id)
        {
            var result = _service.GetDetailsById(id);
            return Convert<HistoryOperationModel>(result);
        }

        /// <summary>
        /// Returns details of the history operation.
        /// </summary>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>Details of the history operation.</returns>
        public async Task<HistoryOperationModel> GetDetailsAsync(string id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _service.GetDetailsByIdAsync(id, cancellationToken);
            return Convert<HistoryOperationModel>(result);
        }

        /// <summary>
        /// Set TxHash to the history operation.
        /// </summary>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="txHash">TxHash of the history operation.</param>
        public void SetTxHash(string id, string txHash)
        {
            var result = _service.SetTxHash(id, txHash);
            Convert<object>(result);
        }

        /// <summary>
        /// Set TxHash to the history operation.
        /// </summary>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="txHash">TxHash of the history operation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        public async Task SetTxHashAsync(string id, string txHash,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _service.SetTxHashAsync(id, txHash, cancellationToken);
            Convert<object>(result);
        }

        /// <summary>
        /// Set Removed attribute to true
        /// </summary>
        /// <param name="id">Identifier of the history operation.</param>
        public void SetRemoved(string id)
        {
            var result = _service.SetRemoved(id);
            Convert<object>(result);
        }

        /// <summary>
        /// Set Removed attribute to true
        /// </summary>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        public async Task SetRemovedAsync(string id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _service.SetRemovedAsync(id, cancellationToken);
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
