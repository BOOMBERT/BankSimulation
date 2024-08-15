using Serilog;

namespace BankSimulation.API.Configuration
{
    public static class LoggingConfiguration
    {
        public static void AddLoggingConfiguration(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
