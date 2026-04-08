using System.Security.Claims;

namespace Tikka.ServicesAustralia.Services;

public interface ITokenService
{
    void AddAccessToken(string serviceProviderName, string token, DateTime expiryTime);
    Tuple<DateTime, string> retrieveAccessTokenIfValid(string serviceProviderName);
}