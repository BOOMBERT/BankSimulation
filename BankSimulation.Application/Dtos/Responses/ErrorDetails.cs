namespace BankSimulation.Application.Dtos.Responses
{
    public record ErrorDetails(string Title, int Status, string Detail, string Instance);
}
