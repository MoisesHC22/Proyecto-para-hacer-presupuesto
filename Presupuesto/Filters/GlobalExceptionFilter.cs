using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace Presupuesto.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var logger = LogManager.GetCurrentClassLogger();

            //Registra la excepción con NLog
            logger.Error(context.Exception, "Excepción no manejada");

            //Devuelve una respuesta de error genérica al cliente
            var result = new ObjectResult(new { error = "Ocurrio un error interno en el servidor" })
            {
                StatusCode = 500
            };
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
