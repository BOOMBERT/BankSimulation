using BankSimulation.API.Middlewares;
using BankSimulation.Application.Exceptions;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Infrastructure.Repositories;
using BankSimulation.Infrastructure.Services;
using BankSimulation.Infrastructure.Services.Utils;
using FluentValidation;
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

            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssembly(applicationAssembly);

            services.AddAutoMapper(applicationAssembly);

            services.AddHttpClient();


            services.AddSingleton<IAuthService, AuthService>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                return new AuthService(
                    config["JwtSettings:Key"] ?? throw new ArgumentNullException("The JwtSettingsKey cannot be null."),
                    config["JwtSettings:AccessToken:ExpirationInMinutes"] 
                    ?? throw new ArgumentNullException("The JwtSettingsAccessTokenExpirationInMinutes cannot be null."), 
                    config["JwtSettings:RefreshToken:ExpirationInMinutes"] 
                    ?? throw new ArgumentNullException("The JwtSettingsRefreshTokenExpirationInMinutes cannot be null.")
                    );
            });
            
            services.AddSingleton<IMoneyOperationsService, MoneyOperationsService>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var config = provider.GetRequiredService<IConfiguration>();
                return new MoneyOperationsService(
                    httpClientFactory.CreateClient(),
                    config["ExchangeCurrenciesSettings:ApiKey"] ?? throw new ArgumentNullException("The ExchangeCurrenciesApiKey cannot be null."),
                    config["ExchangeCurrenciesSettings:ApiUrl"] ?? throw new ArgumentNullException("The ExchangeCurrenciesApiUrl cannot be null.")
                    );
            });


            services.AddTransient<ErrorHandlingMiddleware>();


            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRepository, UserRepository>();
            
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

            services.AddScoped<IBankAccountOperationsService, BankAccountOperationsService>();
        }
    }
}
