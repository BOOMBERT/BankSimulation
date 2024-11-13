namespace BankSimulation.Application.Common.Dtos
{
    public record ErrorDetails(string Title, int Status, object Details, string Instance);
}
