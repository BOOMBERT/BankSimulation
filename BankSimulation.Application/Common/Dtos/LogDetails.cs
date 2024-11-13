namespace BankSimulation.Application.Common.Dtos
{
    public record LogDetails(string Method, string RequestPath, string? ErrorContext, object Details, string QueryParams, string LogId);
}
