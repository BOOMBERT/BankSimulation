namespace BankSimulation.Application.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public string? Email { get; init; }

        public EmailAlreadyExistsException() : base("This email address already exists.") { }
        public EmailAlreadyExistsException(string email) : base($"Email address '{email}' already exists.") 
        {
            Email = email;
        }
    }
}
