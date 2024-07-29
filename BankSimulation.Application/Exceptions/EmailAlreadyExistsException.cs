namespace BankSimulation.Application.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException() : base("This email address already exists.") { }
        public EmailAlreadyExistsException(string email) : base($"Email '{email}' is already registered.") { }
    }
}
