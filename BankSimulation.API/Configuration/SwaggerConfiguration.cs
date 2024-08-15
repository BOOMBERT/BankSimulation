using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace BankSimulation.API.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddConfigurationSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
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
