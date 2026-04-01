using System.Text;
using Tikka.ServicesAustralia.Configs;
using Tikka.ServicesAustralia.Middlewares;
using Tikka.ServicesAustralia.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Tikka.ServicesAustralia;

public static class ConfigRegister
{
    public static IServiceCollection RegisterConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging();
        

        // Authentication
        services.AddSingleton<IAccessTokenService, AccessTokenService>();
        services.AddScoped<JwtMiddleware>();

        JwtConfig jwtConfig = new();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


        // Services Australia Device Configurations
        ServicesAustraliaDeviceConfig saDeviceConfig = new();
        configuration.GetSection("ServicesAustraliaDevice").Bind(saDeviceConfig);
        services.AddSingleton(saDeviceConfig);

        return services;
    }
}