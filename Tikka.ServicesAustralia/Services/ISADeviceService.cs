using System.Security.Claims;

namespace Tikka.ServicesAustralia.Services;

public interface ISADeviceService
{
    string GetDeviceInfo();
    string Activate(string activationCode);
}