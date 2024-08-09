namespace BankSimulation.Application.Dtos.Responses
{
    public class ErrorDetails
    {
        public required string Title { get; init; }
        public required int Status { get; init; }
        public required string Detail { get; init; }
        public required string Instance { get; init; }
    }
}
