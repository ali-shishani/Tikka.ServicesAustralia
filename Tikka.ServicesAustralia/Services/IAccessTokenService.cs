using System.Security.Claims;

namespace Tikka.ServicesAustralia.Services;

public interface IAccessTokenService
{
    ClaimsPrincipal? GetPrincipalFromToken(string token);
}