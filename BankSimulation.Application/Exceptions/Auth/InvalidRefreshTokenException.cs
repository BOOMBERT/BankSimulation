using BankSimulation.Application.Exceptions;

public class InvalidRefreshTokenException : CustomException
{
    public InvalidRefreshTokenException(
        string title = "Invalid Refresh Token",
        string message = "The refresh token provided is invalid.")
        : base(message, title, 401) { }
}