namespace BankSimulation.Application.Exceptions.User
{
    public sealed class EmailAlreadyRegisteredException : CustomException
    {
        public EmailAlreadyRegisteredException(
            string title = "Email Registration Error",
            string message = "This email address is already registered.")
            : base(message, title, 409) { }
    }
}
