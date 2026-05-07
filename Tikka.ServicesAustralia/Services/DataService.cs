
using System.Security.Cryptography;
using Newtonsoft.Json;
using Tikka.ServicesAustralia.Core.Configs;
using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Data.Repositories;
using Tikka.ServicesAustralia.Models.Requests;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Utilities;

namespace Tikka.ServicesAustralia.Services;

public class DataService : IDataService
{
    private ServicesAustraliaDeviceConfig _servicesAustraliaDeviceConfig { get; set; }

    private IStagedCareRecipientRepository _stagedCareRecipientRepository { get; set; }

    private IEventRepository _eventRepository { get; set; }

    private IAuthenticationService _authenticationService { get; set; }

    private HTTPUtility httpUtil { get; set; } = new HTTPUtility(string.Empty, string.Empty, string.Empty);

    public DataService(
        ServicesAustraliaDeviceConfig servicesAustraliaDeviceConfig,
        IStagedCareRecipientRepository stagedCareRecipientRepository,
        IEventRepository eventRepository,
        IAuthenticationService authenticationService
        )
    {
        _servicesAustraliaDeviceConfig = servicesAustraliaDeviceConfig;
        _stagedCareRecipientRepository = stagedCareRecipientRepository;
        _eventRepository = eventRepository;
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
            _servicesAustraliaDeviceConfig.BaseUrl,
            careRecipientId,
            firstName,
            middleName,
            lastName,
            gender,
            birthDate,
            postCode,
            State);

            result = JsonConvert.DeserializeObject<CareRecipientSearchResponse>(response);

            if (result != null)
            {
                try
                {
                    var record = await _stagedCareRecipientRepository.AddRecordAsync(new StagedCareRecipient
                    {
                        CareRecipientId = result.CareRecipientId,
                        FirstName = result.FirstName,
                        MiddleName = result.MiddleName,
                        LastName = result.LastName,
                        Gender = result.Gender,
                        BirthDate = result.BirthDate,
                        TempAccessKey = result.TempAccessKey,
                        TempAccessExpiry = result.TempAccessExpiry,
                    });

                    await _stagedCareRecipientRepository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        else
        {
            result = null;
        }

        return await Task.FromResult(result);
    }

    public async Task<List<QueryEntryEventsResponse>> QueryResidentialEntryEvents(
            string? careRecipientId,
            string? externalReferenceId,
            string? entryDateFrom,
            string? entryDateTo,
            int limit,
            string? page,
            string? sort)
    {
        var result = new List<QueryEntryEventsResponse>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var response = await httpUtil.executeQueryEntryEvents(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId,
            careRecipientId,
            externalReferenceId,
            entryDateFrom,
            entryDateTo,
            limit,
            page,
            sort);

            result = JsonConvert.DeserializeObject<List<QueryEntryEventsResponse>>(response);
        }

        return await Task.FromResult(result);
    }

    public async Task<List<QueryEntryEventsResponse>> QueryHomeEntryEvents(
            string? careRecipientId,
            string? externalReferenceId,
            string? entryDateFrom,
            string? entryDateTo,
            int limit,
            string? page,
            string? sort)
    {
        var result = new List<QueryEntryEventsResponse>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var response = await httpUtil.executeQueryEntryEvents(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareHomeServiceId,
            careRecipientId,
            externalReferenceId,
            entryDateFrom,
            entryDateTo,
            limit,
            page,
            sort);

            result = JsonConvert.DeserializeObject<List<QueryEntryEventsResponse>>(response);
        }

        return await Task.FromResult(result);
    }

    public async Task<GetEntryEventDetailsResponse> GetEntryEventDetails(string? eventId)
    {
        var result = new GetEntryEventDetailsResponse();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var response = await httpUtil.executeGetEntryEventDetails(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId,
            eventId);

            result = JsonConvert.DeserializeObject<GetEntryEventDetailsResponse>(response);
        }

        return await Task.FromResult(result);
    }

    public async Task<CreateEntryEventResponse> CreateEntryEvent(string tempAccessKey, CreateEntryEventRequest request)
    {
        var result = new CreateEntryEventResponse();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var (response, etag) = await httpUtil.executeCreateEntryEvent(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            request,
            tempAccessKey,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId);
            result = JsonConvert.DeserializeObject<CreateEntryEventResponse>(response);

            // save the event to the database
            if (result != null)
            {
                await _eventRepository.AddRecordAsync(new Event() { CareRecipientId = request.CareRecipientId, EventId = result.EventId, Etag = etag });
                await _eventRepository.SaveChangesAsync();
            }
        }

        return await Task.FromResult(result);
    }

    public async Task<UpdateEntryEventResponse> UpdateEntryEvent(string? eventId, UpdateEntryEventRequest request)
    {
        var result = new UpdateEntryEventResponse();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var response = await httpUtil.executeUpdateEntryEvent(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            eventId,
            request,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId);
            result = JsonConvert.DeserializeObject<UpdateEntryEventResponse>(response);
        }

        return await Task.FromResult(result);
    }

    public async Task<DeleteEntryEventResponse> DeleteEntryEvent(string? eventId)
    {
        var result = new DeleteEntryEventResponse();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            // get the etag
            var record = await _eventRepository.GetByEventIdAsync(eventId);
            if (record != null)
            {

                var response = await httpUtil.executeDeleteEntryEvent(
                _servicesAustraliaDeviceConfig.OrganisationRA,
                _servicesAustraliaDeviceConfig.DeviceName,
                _servicesAustraliaDeviceConfig.ProductId,
                accessToken,
                _servicesAustraliaDeviceConfig.BaseUrl,
                _servicesAustraliaDeviceConfig.ServiceNapsId,
                _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId,
                eventId,
                record.Etag);

                result = JsonConvert.DeserializeObject<DeleteEntryEventResponse>(response);

                // delete the event in the database
                if (result != null)
                {
                    await _eventRepository.DeleteByEventIdAsync(eventId);
                    await _eventRepository.SaveChangesAsync();
                }
            }
        }

        return await Task.FromResult(result);
    }

    public async Task<List<EntryEventHistoryResponse>> EntryEventHistory(string? eventId)
    {
        var result = new List<EntryEventHistoryResponse>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var response = await httpUtil.executeEntryEventHistory(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId,
            eventId);

            result = JsonConvert.DeserializeObject<List<EntryEventHistoryResponse>>(response);
        }

        return await Task.FromResult(result);
    }
}