namespace BankSimulation.Application.Dtos.Responses
{
    public record ErrorDetails(string Title, int Status, object Details, string Instance);
}
