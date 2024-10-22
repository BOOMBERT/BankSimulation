using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.DbContexts.Configurations
{
    public sealed class SecurityQuestionConfiguration : IEntityTypeConfiguration<SecurityQuestion>
    {
        public void Configure(EntityTypeBuilder<SecurityQuestion> builder)
        {
            builder.Property(sq => sq.Question)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(sq => sq.Answer)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
