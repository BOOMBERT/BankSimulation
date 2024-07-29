namespace BankSimulation.Domain.Entities
{
    public class DeletedUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime Deleted { get; set; } = DateTime.UtcNow;
    }
}
