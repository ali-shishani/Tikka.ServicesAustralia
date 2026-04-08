using System.Text;
using Tikka.ServicesAustralia.Configs;
using Tikka.ServicesAustralia.Middlewares;
using Tikka.ServicesAustralia.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Tikka.ServicesAustralia;

public static class ServiceRegister
{
    public static IServiceCollection RegisterTikkaServices(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<ISADeviceService, SADeviceService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        return services;
    }
}