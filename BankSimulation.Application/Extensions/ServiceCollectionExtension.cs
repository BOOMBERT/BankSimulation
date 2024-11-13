using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Application.Auth.Services;
using BankSimulation.Application.Common.Interfaces;
using BankSimulation.Application.Common.Services;
using Microsoft.Extensions.Configuration;
using BankSimulation.Application.Users.Interfaces;
using BankSimulation.Application.Users.Services;
using BankSimulation.Application.BankAccounts.Interfaces;
using BankSimulation.Application.BankAccounts.Services;
using BankSimulation.Application.SecurityQuestions.Interfaces;
using BankSimulation.Application.SecurityQuestions.Services;

namespace BankSimulation.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var applicationAssembly = typeof(ServiceCollectionExtension).Assembly;

            services.AddAutoMapper(applicationAssembly);

            services.AddValidatorsFromAssembly(applicationAssembly)
                .AddFluentValidationAutoValidation();

            services.AddHttpClient();

            services.AddSingleton<ITokenService, TokenService>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                return new TokenService(
                    config["JwtSettings:Key"] 
                        ?? throw new ArgumentNullException("The JwtSettingsKey cannot be null."),
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

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminUserService, AdminUserService>();

            services.AddScoped<IAdminSecurityQuestionService, AdminSecurityQuestionService>();
            services.AddScoped<ISecurityQuestionService, SecurityQuestionService>();

            services.AddScoped<IAdminBankAccountService, AdminBankAccountService>();
            services.AddScoped<IBankAccountService, BankAccountService>();

            services.AddScoped<IAdminBankAccountOperationsService, AdminBankAccountOperationsService>();
            services.AddScoped<IBankAccountOperationsService, BankAccountOperationsService>();
        }
    }
}
