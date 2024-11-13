using BankSimulation.API.Middlewares;
using BankSimulation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace BankSimulation.API.Extensions
{
    internal static class WebApplicationExtension
    {
        internal static void UseStartupConfiguration(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            ApplyMigrations(app);
        }

        private static void RunInDbContextScope(IApplicationBuilder app, Action<AppDbContext> action)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            action(dbContext);
        }

        private static void ApplyMigrations(IApplicationBuilder app)
        {
            RunInDbContextScope(app, dbContext =>
            {
                dbContext.Database.Migrate();
            });
        }
    }
}
