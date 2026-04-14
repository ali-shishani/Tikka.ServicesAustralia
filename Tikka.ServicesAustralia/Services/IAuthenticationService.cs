using System.Security.Claims;

namespace Tikka.ServicesAustralia.Services;

public interface IAuthenticationService
{
    (string log, string accessToken) GetAccessToken(bool forceNewToken, string audience = null);
}