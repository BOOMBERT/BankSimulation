using BankSimulation.API.Middlewares;
using BankSimulation.Application.Exceptions;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Infrastructure.Repositories;
using BankSimulation.Infrastructure.Services;
using BankSimulation.Infrastructure.Services.Utils;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace BankSimulation.API.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddConfigurationServices(this IServiceCollection services)
        {
            Assembly applicationAssembly = Assembly.Load("BankSimulation.Application");

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation(x =>
                {
                    x.RegisterValidatorsFromAssembly(applicationAssembly);
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(ms => ms.Value!.Errors.Count > 0)
                            .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                            );

                        throw new ValidationErrorException(errors);
                    };
                });

            services.AddAutoMapper(applicationAssembly);

            services.AddHttpClient();

            services.AddScoped<ErrorHandlingMiddleware>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAuthService, AuthService>();
            
            services.AddScoped<IUserAuthService, UserAuthService>();

            services.AddScoped<IAdminUserService, AdminUserService>();

            services.AddScoped<ISecurityQuestionRepository, SecurityQuestionRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IAdminSecurityQuestionService, AdminSecurityQuestionService>();

            services.AddScoped<ISecurityQuestionService, SecurityQuestionService>();

            services.AddScoped<IAdminBankAccountService, AdminBankAccountService>();

            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            services.AddScoped<IBankAccountService, BankAccountService>();

            services.AddScoped<IAdminBankAccountOperationsService, AdminBankAccountOperationsService>();

            services.AddScoped<IBankAccountOperationsRepository, BankAccountOperationsRepository>();
        }
    }
}
