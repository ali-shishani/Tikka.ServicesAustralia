using System.Text;
using Tikka.ServicesAustralia.Core.Configs;
using Tikka.ServicesAustralia.Core.Middlewares;
using Tikka.ServicesAustralia.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tikka.ServicesAustralia.Core.Data;
using Microsoft.EntityFrameworkCore;
using Tikka.ServicesAustralia.Core.Data.Repositories;

namespace Tikka.ServicesAustralia;

public static class ServiceRegister
{
    public static IServiceCollection RegisterTikkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<TikkaDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")
                              ?? configuration.GetConnectionString("DockerConnection"));
        });

        // Services Australia Device Configurations
        ServicesAustraliaDeviceConfig saDeviceConfig = new();
        configuration.GetSection("ServicesAustraliaDevice").Bind(saDeviceConfig);
        services.AddSingleton(saDeviceConfig);

        // Repositories
        services.AddScoped<IStoredInfoRepository, StoredInfoRepository>();

        // Services
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<ISADeviceService, SADeviceService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IDataService, DataService>();

        return services;
    }
}