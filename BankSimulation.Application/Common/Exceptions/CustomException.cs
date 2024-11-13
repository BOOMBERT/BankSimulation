using BankSimulation.Application.Common.Interfaces;

namespace BankSimulation.Application.Common.Exceptions
{
    public class CustomException : Exception, ICustomException
    {
        public string Title { get; }
        public int StatusCode { get; }
        public object Details { get; }
        public string? ErrorContext { get; }
        public CustomException(string title, int statusCode, object details, string? errorContext = null) : base()
        {
            Title = title;
            StatusCode = statusCode;
            Details = details;
            ErrorContext = errorContext;
        }
    }
}
