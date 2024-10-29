using BankSimulation.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Configuration
{
    public static class DbContextConfiguration
    {
        public static void AddConfigurationDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
