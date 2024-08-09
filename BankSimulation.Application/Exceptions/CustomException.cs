using BankSimulation.Application.Interfaces;

namespace BankSimulation.Application.Exceptions
{
    public class CustomException : Exception, ICustomException
    {
        public string Title { get; }
        public int StatusCode { get; }

        public CustomException(string message, string title, int statusCode)
            : base(message)
        {
            Title = title;
            StatusCode = statusCode;
        }
    }
}
