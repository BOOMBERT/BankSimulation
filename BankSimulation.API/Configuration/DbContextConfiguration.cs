using BankSimulation.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Configuration
{
    public static class DbContextConfiguration
    {
        public static void AddConfigurationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsersContext>(options 
                => options.UseSqlServer(configuration["ConnectionStrings:BankSimulationConnectionString"]));
        }
    }
}
