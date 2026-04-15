using System.Text;
using Tikka.ServicesAustralia.Core.Configs;
using Tikka.ServicesAustralia.Core.Middlewares;
using Tikka.ServicesAustralia.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Tikka.ServicesAustralia.Core;

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

        return services;
    }
}