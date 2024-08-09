namespace BankSimulation.Application.Exceptions.Auth
{
    public class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException(
        string title = "Invalid Credentials",
        string message = "The provided credentials are invalid. Please check your address email and password and try again.")
        : base(message, title, 401) { }
    }
}
