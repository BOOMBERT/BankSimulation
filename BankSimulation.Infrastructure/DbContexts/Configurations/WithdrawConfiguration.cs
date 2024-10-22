using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.DbContexts.Configurations
{
    public sealed class WithdrawConfiguration : IEntityTypeConfiguration<Withdraw>
    {
        public void Configure(EntityTypeBuilder<Withdraw> builder)
        {
            builder.Property(w => w.Amount)
                .HasPrecision(18, 2);
        }
    }
}
