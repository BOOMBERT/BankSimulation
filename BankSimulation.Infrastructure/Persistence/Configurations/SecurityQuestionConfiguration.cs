using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Persistence.Configurations
{
    internal sealed class SecurityQuestionConfiguration : IEntityTypeConfiguration<SecurityQuestion>
    {
        public void Configure(EntityTypeBuilder<SecurityQuestion> builder)
        {
            builder.Property(sq => sq.Question)
                .HasMaxLength(128);

            builder.Property(sq => sq.Answer)
                .HasMaxLength(255);
        }
    }
}
