using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAccess.API.Middlewere
{



    public class ExceptionHandlingMiddlewere
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddlewere> _logger;

        public ExceptionHandlingMiddlewere(RequestDelegate next, ILogger<ExceptionHandlingMiddlewere> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new { message = "An internal server error ocurred." };

                var json = System.Text.Json.JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);

            }

        }

    }
}