using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasOne(u => u.SecurityQuestion)
                .WithOne(sq => sq.User)
                .HasForeignKey<SecurityQuestion>(sq => sq.UserId);

            builder.HasMany(u => u.BankAccounts)
                .WithOne(ba => ba.User)
                .HasForeignKey(ba => ba.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.RefreshToken)
                .WithOne(rt => rt.User)
                .HasForeignKey<RefreshToken>(rt => rt.UserId);

            builder.HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
