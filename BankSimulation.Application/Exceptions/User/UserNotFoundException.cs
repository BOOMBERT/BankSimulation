namespace BankSimulation.Application.Exceptions.User
{
    public class UserNotFoundException : CustomException
    {
        public UserNotFoundException(
            string title = "User Not Found",
            string message = "The specified user could not be found.")
            : base(message, title, 404) { }
    }
}
