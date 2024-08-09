namespace BankSimulation.Application.Exceptions.User
{
    public class UserAlreadyDeletedException : CustomException
    {
        public UserAlreadyDeletedException(
            string title = "User Already Deleted",
            string message = "The specified user has already been deleted.")
            : base(message, title, 409) { }

    }
}
