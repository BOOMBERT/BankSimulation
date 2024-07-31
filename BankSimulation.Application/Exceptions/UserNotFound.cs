namespace BankSimulation.Application.Exceptions
{
    public class UserNotFound : Exception
    {
        public Guid? UserId { get; init; } = null;

        public UserNotFound() : base("This user does not exist.") { }
        public UserNotFound(Guid id) : base($"User '{id}' does not exist.") 
        {
            UserId = id;
        }
    }
}
