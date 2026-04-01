
using System.Security.Cryptography;
using Newtonsoft.Json;
using Tikka.ServicesAustralia.Configs;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Utilities;

namespace Tikka.ServicesAustralia.Services;

public class AuthenticationService : IAuthenticationService
{

    public string GetAccessToken()
    {
        var accessToken = string.Empty;

        return accessToken;
    }
}