using BankSimulation.API.Middlewares;
using BankSimulation.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.API.Configuration
{
    public static class MiddlewaresConfiguration
    {
        public static void UseConfiguredMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            ApplyMigrations(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();
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
                dbContext.Database.EnsureCreated();

                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            });
        }
    }
}
