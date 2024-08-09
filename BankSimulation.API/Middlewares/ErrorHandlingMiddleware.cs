using BankSimulation.Application.Dtos.Responses;
using BankSimulation.Application.Interfaces;
using System.Text.Json;

namespace BankSimulation.API.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex) when (ex is ICustomException customException)
            {
                context.Response.StatusCode = customException.StatusCode;
                context.Response.ContentType = "application/json";

                var problemDetails = new ErrorDetails
                {
                    Title = customException.Title,
                    Status = customException.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                var responseJson = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}
