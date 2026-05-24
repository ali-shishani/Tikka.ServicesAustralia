
using Newtonsoft.Json;
using System.Security.Cryptography;
using Tikka.ServicesAustralia.Core.Configs;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;
using Tikka.ServicesAustralia.Core.Data.Repositories;
using Tikka.ServicesAustralia.Core.Models;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Utilities;

namespace Tikka.ServicesAustralia.Services;

public class SADeviceService : ISADeviceService
{

    private ServicesAustraliaDeviceConfig _servicesAustraliaDeviceConfig { get; set; }

    private IStoredInfoRepository _storedInfoRepository { get; set; }

    private IAuthenticationService _authenticationService { get; set; }

    private HTTPUtility httpUtil { get; set; } = new HTTPUtility(string.Empty, string.Empty, string.Empty);

    private RSAKeyUtility rsaKeyUtility { get; set; } = new RSAKeyUtility();


    public SADeviceService(
        IStoredInfoRepository storedInfoRepository,
        ServicesAustraliaDeviceConfig servicesAustraliaDeviceConfig,
        IAuthenticationService authenticationService)
    {
        _storedInfoRepository = storedInfoRepository;
        _servicesAustraliaDeviceConfig = servicesAustraliaDeviceConfig;
        _authenticationService = authenticationService;
    }

    public async Task<(DeviceInformationResponse response, List<AppException> errors)> GetDeviceInfo()
    {
        var result = new DeviceInformationResponse();
        var errors = new List<AppException>();

        result.ClientId = _servicesAustraliaDeviceConfig.ClientId;
        result.ProductId = _servicesAustraliaDeviceConfig.ProductId;
        result.DeviceName = _servicesAustraliaDeviceConfig.DeviceName;
        result.OrganisationRA = _servicesAustraliaDeviceConfig.OrganisationRA;
        result.ServiceNapsId = _servicesAustraliaDeviceConfig.ServiceNapsId;
        result.AgedCareResidentialServiceId = _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId;
        result.AgedCareHomeServiceId = _servicesAustraliaDeviceConfig.AgedCareHomeServiceId;
        result.TokenAud = _servicesAustraliaDeviceConfig.TokenAud;

        var keyExpiration = await _storedInfoRepository.GetByCodeAsync(StoredInfoCode.KeyExpiration);
        result.KeyExpiry = keyExpiration?.Value;

        var deviceExpiration = await _storedInfoRepository.GetByCodeAsync(StoredInfoCode.DeviceExpiration);
        result.DeviceExpiry = deviceExpiration?.Value;

        return (result, errors);
    }

    public async Task<(string response, List<AppException> errors)> Activate(string activationCode)
    {
        var errors = new List<AppException>();
        var logText = "Device activation started";

        if (string.IsNullOrWhiteSpace(activationCode))
        {
            errors.Add(new AppException("Activation code is required", "Activation code is required"));
            return (logText, errors);
        }

        // generate a new key
        rsaKeyUtility.createKeys(_servicesAustraliaDeviceConfig.DeviceName);
        logText = "RSA key generated";

        // get public key in JWK format
        var publicJwk = rsaKeyUtility.generatePublicJwk(_servicesAustraliaDeviceConfig.DeviceName);
        logText += Environment.NewLine + "RSA key public JWK: " + publicJwk;

        // build the request body
        var reqBody = httpUtil.buildActivationRequest(_servicesAustraliaDeviceConfig.OrganisationRA, activationCode, publicJwk);
        logText += Environment.NewLine + "reqBody: " + reqBody;

        // execute the HTTP request
        var result = httpUtil.executeActivateDeviceRequest(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            reqBody);

        logText += Environment.NewLine + "response: " + Environment.NewLine + "------------------------" + Environment.NewLine + result + Environment.NewLine + "------------------------";

        // determine if request was successful
        if (result.Contains("ACTIVE"))
        {
            // convert response to object
            var responseObject = JsonConvert.DeserializeObject<ActivateResponse>(result);
            logText += Environment.NewLine + "Key Expiration: " + responseObject?.keyExpiry;
            logText += Environment.NewLine + "Device Expiration: " + responseObject?.deviceExpiry;

            await _storedInfoRepository.UpsertByCodeAsync(StoredInfoCode.KeyExpiration, responseObject?.keyExpiry);
            await _storedInfoRepository.UpsertByCodeAsync(StoredInfoCode.DeviceExpiration, responseObject?.deviceExpiry);
        }

        return (logText, errors);
    }

    public async Task<(string response, List<AppException> errors)> RefreshKey()
    {
        var errors = new List<AppException>();
        var logText = "Device refresh key process started";

        // generate a new key, but dont persist in the key store
        var newKey = rsaKeyUtility.createKeys(_servicesAustraliaDeviceConfig.DeviceName, false);
        logText += "New RSA key generated";

        // get the public key in jwk format
        var publicJwk = rsaKeyUtility.generatePublicJwk(_servicesAustraliaDeviceConfig.DeviceName, newKey);
        logText += Environment.NewLine + "RSA key public JWK: " + publicJwk;


        // get an access token for this call.
        // NOTE: this call is an example of using a PRODA access token to
        // authentcate to a 3rd party.  See HttpUtility class for example of putting
        // the token in the authorization header.
        var (log, accessToken) = _authenticationService.GetAccessToken(false);


        // execute the HTTP request if we have an access token
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            logText += Environment.NewLine + "Access token acquired: " + accessToken;
            var result = httpUtil.executeRefreshKeyRequest(
                _servicesAustraliaDeviceConfig.OrganisationRA,
                _servicesAustraliaDeviceConfig.DeviceName,
                _servicesAustraliaDeviceConfig.ProductId,
                publicJwk,
                accessToken);
            logText += Environment.NewLine + "response: " + Environment.NewLine + "------------------------" + Environment.NewLine + result + Environment.NewLine + "------------------------";

            // determine if request was successful
            if (result.Contains("ACTIVE"))
            {
                // convert response to object
                var responseObject = JsonConvert.DeserializeObject<ActivateResponse>(result);
                logText += Environment.NewLine + "Key Expiration: " + responseObject?.keyExpiry;
                logText += Environment.NewLine + "Device Expiration: " + responseObject?.deviceExpiry;

                // update the key store
                rsaKeyUtility.persistRsaKey(newKey, _servicesAustraliaDeviceConfig.DeviceName);

                await _storedInfoRepository.UpsertByCodeAsync(StoredInfoCode.KeyExpiration, responseObject?.keyExpiry);
                await _storedInfoRepository.UpsertByCodeAsync(StoredInfoCode.DeviceExpiration, responseObject?.deviceExpiry);
            }
        }
        else
        {
            logText += Environment.NewLine + "Failed to acquire access token.";
        }

        return (logText, errors);
    }
}