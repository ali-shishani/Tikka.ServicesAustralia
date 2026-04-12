
using System.Security.Cryptography;
using Newtonsoft.Json;
using Tikka.ServicesAustralia.Configs;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Utilities;

namespace Tikka.ServicesAustralia.Services;

public class SADeviceService : ISADeviceService
{

    private ServicesAustraliaDeviceConfig _servicesAustraliaDeviceConfig { get; set; }

    private HTTPUtility httpUtil { get; set; } = new HTTPUtility(string.Empty, string.Empty, string.Empty);

    private RSAKeyUtility rsaKeyUtility { get; set; } = new RSAKeyUtility();
    

    public SADeviceService(ServicesAustraliaDeviceConfig servicesAustraliaDeviceConfig)
    {
        _servicesAustraliaDeviceConfig = servicesAustraliaDeviceConfig;
    }

    public string GetDeviceInfo()
    {
        var deviceInfo = "ClientId:" + _servicesAustraliaDeviceConfig.ClientId +
             Environment.NewLine + "Product Id:" + _servicesAustraliaDeviceConfig.ProductId +
             Environment.NewLine + "Device Name:" + _servicesAustraliaDeviceConfig.DeviceName +
             Environment.NewLine + "Organisation RA:" + _servicesAustraliaDeviceConfig.OrganisationRA;

        return deviceInfo;
    }

    public string Activate(string activationCode)
    {
        if (string.IsNullOrWhiteSpace(activationCode))
        {
            return "Activation code is required.";
        }

        // generate a new key
        rsaKeyUtility.createKeys(_servicesAustraliaDeviceConfig.DeviceName);
        var logText = "RSA key generated";

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
        }

        return logText;
    }

    public string RefreshKey()
    {
        var result = string.Empty;

        return result;
    }
}