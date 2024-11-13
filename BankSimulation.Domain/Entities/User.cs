using BankSimulation.Domain.Enums;

namespace BankSimulation.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public IList<AccessRole> AccessRoles { get; set; } = new List<AccessRole>();

        public RefreshToken RefreshToken { get; set; }
        public SecurityQuestion SecurityQuestion { get; set; }
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    }
}
