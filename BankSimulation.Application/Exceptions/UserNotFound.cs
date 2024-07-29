namespace BankSimulation.Application.Exceptions
{
    public class UserNotFound : Exception
    {
        public UserNotFound() : base("This user does not exist.") { }
        public UserNotFound(Guid id) : base($"User '{id}' does not exist.") { }
    }
}
