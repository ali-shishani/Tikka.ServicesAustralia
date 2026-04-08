using System.Security.Claims;

namespace Tikka.ServicesAustralia.Services;

public interface IAuthenticationService
{
    string GetAccessToken(bool forceNewToken, string audience = null);
}