using BankSimulation.Domain.Repositories;
using BankSimulation.Infrastructure.Persistence;
using BankSimulation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankSimulation.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BankSimulationDatabase");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ISecurityQuestionRepository, SecurityQuestionRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            services.AddScoped<IBankAccountOperationsRepository, BankAccountOperationsRepository>();
        }
    }
}
