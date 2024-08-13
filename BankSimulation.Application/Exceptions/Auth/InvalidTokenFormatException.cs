namespace BankSimulation.Application.Exceptions.Auth
{
    public sealed class InvalidTokenFormatException : CustomException
    {
        public InvalidTokenFormatException(
            string title = "Invalid Token Format",
            string message = "The provided token format is invalid.")
            : base(message, title, 400) { }
    }
}
