namespace BankSimulation.Application.Dtos.Responses
{
    public class ErrorResponse
    {
        public required string Title { get; init; }
        public required string Detail { get; init; }
    }
}
