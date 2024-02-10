using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PhoenixAuth
{
    public class RequestAsyncActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<RequestAsyncActionFilter> _logger;

        public RequestAsyncActionFilter(ILogger<RequestAsyncActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            
            LogResponse(resultContext);

        }

        private void LogResponse(ActionExecutedContext resultContext)
        {

            var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            if (resultContext.Exception == null)
            {
                var result = (ObjectResult)resultContext.Result;

                _logger.LogInformation("{appName} Response Body is {@ObjectResult}", appName, result.Value);
            }

            //var kld = resultContext.Result.ToJson();
        }
    }
}
