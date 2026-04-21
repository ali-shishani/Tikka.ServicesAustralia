
using System.Security.Cryptography;
using Newtonsoft.Json;
using Tikka.ServicesAustralia.Core.Configs;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Utilities;

namespace Tikka.ServicesAustralia.Services;

public class DataService : IDataService
{
    private ServicesAustraliaDeviceConfig _servicesAustraliaDeviceConfig { get; set; }

    private IAuthenticationService _authenticationService { get; set; }

    private HTTPUtility httpUtil { get; set; } = new HTTPUtility(string.Empty, string.Empty, string.Empty);

    public DataService(
        ServicesAustraliaDeviceConfig servicesAustraliaDeviceConfig,
        IAuthenticationService authenticationService
        )
    {
        _servicesAustraliaDeviceConfig = servicesAustraliaDeviceConfig;
        _authenticationService = authenticationService;
    }

    public async Task<CareRecipientSearchResponse> CareRecipientSearch(
        string? careRecipientId,
        string? firstName,
        string? middleName,
        string? lastName,
        string? gender,
        string? birthDate,
        string? postCode,
        string? State)
    {
        var result = new CareRecipientSearchResponse();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var response = await httpUtil.executeCareRecipientSearch(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            careRecipientId,
            firstName,
            middleName,
            lastName,
            gender,
            birthDate,
            postCode,
            State);

            result = JsonConvert.DeserializeObject<CareRecipientSearchResponse>(response);
        }
        else
        {
            result = null;
        }

        return await Task.FromResult(result);
    }

    public async Task<ResidentialCareEntryEventResponse> ResidentialCareEntry()
    {
        var result = new ResidentialCareEntryEventResponse();

        return await Task.FromResult(result);
    }
}