using BankSimulation.API.Middlewares;

namespace BankSimulation.API.Configuration
{
    public static class MiddlewaresConfiguration
    {
        public static void UseConfiguredMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();
        }
    }
}
