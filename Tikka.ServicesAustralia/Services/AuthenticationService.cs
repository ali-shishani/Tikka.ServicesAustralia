
using System.Security.Cryptography;
using Newtonsoft.Json;
using Tikka.ServicesAustralia.Configs;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Utilities;

namespace Tikka.ServicesAustralia.Services;

public class AuthenticationService : IAuthenticationService
{
    private ServicesAustraliaDeviceConfig _servicesAustraliaDeviceConfig { get; set; }

    private ITokenService _tokenService { get; set; }

    private RSAKeyUtility rsaKeyUtility { get; set; } = new RSAKeyUtility();

    private JWTUtility jwtUtility { get; set; } = new JWTUtility();

    private HTTPUtility httpUtil { get; set; } = new HTTPUtility(string.Empty, string.Empty, string.Empty);


    public AuthenticationService(
        ServicesAustraliaDeviceConfig servicesAustraliaDeviceConfig,
        ITokenService tokenService
        )
    {
        _tokenService = tokenService;
        _servicesAustraliaDeviceConfig = servicesAustraliaDeviceConfig;
    }

    public (string log, string accessToken) GetAccessToken(bool forceNewToken, string audience = null)
    {
        var logResult = string.Empty;
        var accessToken = string.Empty;

        // set the token.aud(ience) for the token request
        var tokenAudience = _servicesAustraliaDeviceConfig.TokenAud;
        if (string.IsNullOrWhiteSpace(tokenAudience))
        {
            // this is an override, used by the refresh key call to
            // make PRODA the audience of the token
            tokenAudience = audience;
        }

        // do we have an access token for the required service provider? 
        if (!forceNewToken)
        {
            // check accessTokenStore
            var tokenTuple = _tokenService.retrieveAccessTokenIfValid(tokenAudience);
            if (tokenTuple != null)
            {
                accessToken = tokenTuple.Item2;
                return (logResult, accessToken);
            }
        }

        // generate assertion, first get key
        var rsaCsp = rsaKeyUtility.getCurrentKey(_servicesAustraliaDeviceConfig.DeviceName);

        // create the assertion
        var assertion = jwtUtility.createJWTAssertion(
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.OrganisationRA,
            tokenAudience,
            300, // assertion expires in 5 minutes
            rsaCsp);

        // put together the request body
        var reqBody = httpUtil.buildAccessTokenRequest(_servicesAustraliaDeviceConfig.ClientId, assertion);

        logResult += "reqBody: " + reqBody + Environment.NewLine;

        // get current timestamp, this is used when storing the access token for later use
        var currTimeStamp = DateTime.Now;

        logResult += "current time stamp: " + currTimeStamp.ToLocalTime() + Environment.NewLine;

        // execute the HTTP request
        var result = httpUtil.executeGetAccessTokenRequest(reqBody);

        logResult += "response: " + Environment.NewLine + "------------------------" + Environment.NewLine + result + Environment.NewLine + "------------------------" + Environment.NewLine;

        // determine if request was successful
        if (result.Contains("access_token"))
        {
            // convert response to object
            var responseObject = JsonConvert.DeserializeObject<TokenResponse>(result);

            // update general info display
            // TODO: update in database

            // store the token for reuse
            // calculate expiry time, calculated as time of request + 80% of expires_in value returned by token request
            var expDate = currTimeStamp.Add(new TimeSpan(0, 0, (int)Math.Truncate(responseObject.expires_in * 0.8)));

            logResult += "expiry time stamp for new token: " + expDate.ToLocalTime() + Environment.NewLine;

            // add token to store , the "value" in the dictionary is a tuple of 2 elements; exp time and access token
            _tokenService.AddAccessToken(tokenAudience, responseObject.access_token, expDate);

            logResult += "token added to the token service: " + responseObject.access_token + Environment.NewLine;
            accessToken = responseObject.access_token;
        }
        else
        {
            // for some reason we didnt get an access token,
            // TODO: build logic depending on the error code

                logResult += "for some reason we didnt get an access token" + Environment.NewLine + "Response:" + result;
        }

        return (logResult, accessToken);
    }
}