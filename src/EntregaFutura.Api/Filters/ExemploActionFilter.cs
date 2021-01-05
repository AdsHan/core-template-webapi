using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntregaFutura.Api.Filters
{
    public class ExemploActionFilter : IActionFilter
    {
        private readonly ILogger<ExemploActionFilter> _logger;
        public ExemploActionFilter(ILogger<ExemploActionFilter> logger)
        {
            _logger = logger;
        }
       
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("### OnActionExecuting ###");             
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("### OnActionExecuted ###");
        }
    }
}
