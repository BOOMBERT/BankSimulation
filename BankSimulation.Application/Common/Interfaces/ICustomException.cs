namespace BankSimulation.Application.Common.Interfaces
{
    public interface ICustomException
    {
        public string Title { get; }
        public int StatusCode { get; }
        public string? ErrorContext { get; }
        public object Details { get; }
    }
}
