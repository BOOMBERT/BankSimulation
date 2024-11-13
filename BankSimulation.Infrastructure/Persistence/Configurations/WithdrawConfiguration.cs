using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Persistence.Configurations
{
    internal sealed class WithdrawConfiguration : IEntityTypeConfiguration<Withdraw>
    {
        public void Configure(EntityTypeBuilder<Withdraw> builder)
        {
            builder.Property(w => w.Amount)
                .HasPrecision(18, 2);

            builder.Property(w => w.Date)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
