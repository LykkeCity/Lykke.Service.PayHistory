using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.PayHistory.Filters
{
    public class ErrorResponceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _log;

        public ErrorResponceExceptionFilterAttribute(ILog log)
        {
            _log = log;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await _log.WriteErrorAsync(context.RouteData?.Values["controller"]?.ToString(),
                context.RouteData?.Values["action"]?.ToString(), context.Exception);

            context.Result = new ObjectResult(ErrorResponse.Create(
                $"Internal error: {context.Exception.Message}"))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
