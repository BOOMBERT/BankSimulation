using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Exceptions.Auth;
using BankSimulation.Application.Interfaces;
using Serilog;
using Serilog.Events;
using System.Text.Json;

namespace BankSimulation.API.Middlewares
{
    internal sealed class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    throw new UnauthorizedException();
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    throw new ForbiddenAccessException();
                }
            }
            catch (Exception ex)
            {
                var (statusCode, title, errorContext, details, logLevel) = ex switch
                {
                    ICustomException customException => 
                    (customException.StatusCode, customException.Title, customException.ErrorContext, customException.Details ,LogEventLevel.Error),
                    _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.", null, ex.Message, LogEventLevel.Fatal)
                };

                await LogAndRespondAsync(context, statusCode, title, errorContext, details, logLevel);
            }
        }
        private async Task LogAndRespondAsync(HttpContext context, int statusCode, string title, string? errorContext, object details, LogEventLevel logLevel)
        {
            int maxQueryLength = 256;
            string query = context.Request.QueryString.ToString();

            var log = new LogDetails(
                context.Request.Method,
                context.Request.Path,
                errorContext,
                details,
                query.Length > maxQueryLength ? query[..maxQueryLength] : query,
                context.TraceIdentifier
            );

            Log.Write(logLevel, JsonSerializer.Serialize(log));

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var errorDetails = new ErrorDetails(
                title,
                statusCode,
                details,
                context.Request.Path
            );

            var responseJson = JsonSerializer.Serialize(errorDetails);
            await context.Response.WriteAsync(responseJson);
        }
    }
}
