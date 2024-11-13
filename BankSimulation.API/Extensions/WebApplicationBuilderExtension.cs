using BankSimulation.API.Middlewares;
using BankSimulation.Application.Auth.Exceptions;
using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace BankSimulation.API.Extensions
{
    internal static class WebApplicationBuilderExtension
    {
        internal static void AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ErrorHandlingMiddleware>();

            AddConfiguredControllersBehavior(builder);

            AddConfiguredLogger(builder);
            
            AddConfiguredAuth(builder);

            AddConfiguredSwagger(builder);
        }

        private static void AddConfiguredControllersBehavior(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
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
        }

        private static void AddConfiguredLogger(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration) => 
                configuration.ReadFrom.Configuration(context.Configuration)
            );
        }

        private static void AddConfiguredAuth(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };

                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        throw new InvalidTokenFormatException(context.Request.Headers.Authorization.ToString().Split()[1]);
                    }
                };
            });

            builder.Services.AddAuthorization();
        }

        private static void AddConfiguredSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }
    }
}
