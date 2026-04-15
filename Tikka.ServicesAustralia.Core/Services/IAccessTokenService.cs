using System.Security.Claims;

namespace Tikka.ServicesAustralia.Core.Services;

public interface IAccessTokenService
{
    ClaimsPrincipal? GetPrincipalFromToken(string token);
}