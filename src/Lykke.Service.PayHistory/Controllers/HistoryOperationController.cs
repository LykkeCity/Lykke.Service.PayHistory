using System;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PayHistory.Client.Publisher;
using Lykke.Service.PayHistory.Core.Services;
using Lykke.Service.PayHistory.Filters;
using Lykke.Service.PayHistory.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.PayHistory.Core.Exception;

namespace Lykke.Service.PayHistory.Controllers
{
    [ValidateActionParametersFilter]
    [Route("api/[controller]/[action]")]
    public class HistoryOperationController : Controller
    {
        private readonly IHistoryOperationService _historyOperationService;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public HistoryOperationController(IHistoryOperationService historyOperationService,
            IMapper mapper, ILog log)
        {
            _historyOperationService = historyOperationService;
            _mapper = mapper;
            _log = log;
        }

        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <returns code="200">History operations.</returns>
        /// <returns code="400">Input arguments are invalid.</returns>
        [HttpGet]
        [SwaggerOperation("GetHistory")]
        [ProducesResponseType(typeof(IEnumerable<HistoryOperationViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetHistory(
            [Required, PartitionOrRowKey]string merchantId)
        {
            var results = await _historyOperationService.GetHistoryAsync(merchantId);
            var models = _mapper.Map<IEnumerable<HistoryOperationViewModel>>(results);
            return Ok(models);
        }

        /// <summary>
        /// Returns history operations base info.
        /// </summary>
        /// <param name="invoiceId">Identifier of the invoice.</param>
        /// <returns code="200">History operations.</returns>
        /// <returns code="400">Input arguments are invalid.</returns>
        [HttpGet]
        [SwaggerOperation("GetHistoryByInvoice")]
        [ProducesResponseType(typeof(IEnumerable<HistoryOperationViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetHistoryByInvoice(
            [Required, PartitionOrRowKey]string invoiceId)
        {
            var results = await _historyOperationService.GetHistoryByInvoiceAsync(invoiceId);
            var models = _mapper.Map<IEnumerable<HistoryOperationViewModel>>(results);
            return Ok(models);
        }

        /// <summary>
        /// Returns details of the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <returns code="200">Details of the history operation.</returns>
        /// <returns code="400">Input arguments are invalid.</returns>
        [HttpGet]
        [SwaggerOperation("GetDetails")]
        [ProducesResponseType(typeof(HistoryOperationModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetails(
            [Required, PartitionOrRowKey]string merchantId, 
            [Required, PartitionOrRowKey]string id)
        {
            var result = await _historyOperationService.GetDetailsAsync(merchantId, id);
            if (result == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<HistoryOperationModel>(result);
            return Ok(model);
        }

        /// <summary>
        /// Set TxHash to the history operation.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>
        /// <param name="txHash">TxHash of the history operation.</param>
        /// <returns code="200">Empty successful result.</returns>
        /// <returns code="400">Input arguments are invalid.</returns>
        [HttpPost]
        [SwaggerOperation("SetTxHash")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetTxHash(
            [Required, PartitionOrRowKey]string merchantId, 
            [Required, PartitionOrRowKey]string id, string txHash)
        {
            await _historyOperationService.SetTxHashAsync(merchantId, id, txHash);
            return Ok();
        }

        /// <summary>
        /// Mark history operation as removed
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant.</param>
        /// <param name="id">Identifier of the history operation.</param>s
        /// <response code="200">Empty successful result.</response>
        /// <response code="400">Input arguments are invalid.</response>
        /// <response code="404">History opreration not found</response> 
        [HttpPost]
        [SwaggerOperation("SetRemoved")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> SetRemoved(
            [Required, PartitionOrRowKey] string merchantId,
            [Required, PartitionOrRowKey] string id)
        {
            try
            {
                await _historyOperationService.SetRemovedAsync(merchantId, id);

                return NoContent();
            }
            catch (ArgumentNullException e)
            {
                _log.WriteError(nameof(SetRemoved), new {e.ParamName}, e);

                return BadRequest(ErrorResponse.Create(e.Message));
            }
            catch (HistoryOperationNotFoundException e)
            {
                _log.WriteError(nameof(SetRemoved), new
                {
                    e.MerchantId,
                    e.OperationId
                }, e);

                return NotFound(ErrorResponse.Create(e.Message));
            }
        }
    }
}
