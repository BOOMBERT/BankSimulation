using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.DbContexts.Configurations
{
    public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(rt => rt.ExpirationDate)
                .IsRequired();
        }
    }
}
