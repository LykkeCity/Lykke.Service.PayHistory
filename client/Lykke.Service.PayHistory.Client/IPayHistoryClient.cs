using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.PayHistory.Client.AutorestClient.Models;

namespace Lykke.Service.PayHistory.Client
{
    public interface IPayHistoryClient
    {
        #region IsAliveController

        /// <summary>
        /// Checks service is alive
        /// </summary>
        IsAliveResponse IsAlive();

        /// <summary>
        /// Checks service is alive
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        Task<IsAliveResponse> IsAliveAsync(CancellationToken cancellationToken = default(CancellationToken));
        #endregion IsAliveController

        #region HistoryOperationController

        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <returns>History operations.</returns>
        IEnumerable<HistoryOperationViewModel> GetHistory(string merchantId);

        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>History operations.</returns>
        Task<IEnumerable<HistoryOperationViewModel>> GetHistoryAsync(string merchantId,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="invoiceId">Identifier of the invoice.</param>
        /// <returns>History operations.</returns>
        IEnumerable<HistoryOperationViewModel> GetHistoryByInvoice(string invoiceId);

        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="invoiceId">Identifier of the invoice.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>History operations.</returns>
        Task<IEnumerable<HistoryOperationViewModel>> GetHistoryByInvoiceAsync(string invoiceId,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns details of the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <returns>Details of the history operation.</returns>
        HistoryOperationModel GetDetails(string merchantId, string id);

        /// <summary>
        /// Returns details of the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>Details of the history operation.</returns>
        Task<HistoryOperationModel> GetDetailsAsync(string merchantId, string id,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set TxHash to the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="txHash">TxHash of the history operation.</param>
        void SetTxHash(string merchantId, string id, string txHash);

        /// <summary>
        /// Set TxHash to the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="txHash">TxHash of the history operation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        Task SetTxHashAsync(string merchantId, string id, string txHash,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Removed attribute to true
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        void SetRemoved(string merchantId, string id);

        /// <summary>
        /// Set Removed attribute to true
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        Task SetRemovedAsync(string merchantId, string id,
            CancellationToken cancellationToken = default(CancellationToken));

        #endregion HistoryOperationController
    }
}
