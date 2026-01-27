using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Errors;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                //Para que imprima la excepcion y el mensaje
                _logger.LogError(e, e.Message);

                //se especifica como será La respuesta al cliente del error
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //Create response Object, that sends to customer
                var response = _env.IsDevelopment()
                    ? new CodeErrorException((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace.ToString())
                    : new CodeErrorException((int)HttpStatusCode.InternalServerError);

                //para mantener las propiedades que se devuelven al cliente en minusculas
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response,options);
                
                //To send to Customer
                await context.Response.WriteAsJsonAsync(json);
            }
        }
    }
}
