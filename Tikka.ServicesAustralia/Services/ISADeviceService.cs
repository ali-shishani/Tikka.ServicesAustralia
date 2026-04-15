using System.Security.Claims;

namespace Tikka.ServicesAustralia.Services;

public interface ISADeviceService
{
    Task<string> GetDeviceInfo();
    Task<string> Activate(string activationCode);
    Task<string> RefreshKey();
}