
using System.Security.Cryptography;
using Newtonsoft.Json;
using Tikka.ServicesAustralia.Core.Configs;
using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Data.Repositories;
using Tikka.ServicesAustralia.Core.Models;
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

    public async Task<(CareRecipientSearchResponse response, List<AppException> errors)> CareRecipientSearch(
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
        var errors = new List<AppException>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var (success, response) = await httpUtil.executeCareRecipientSearch(
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

            if (success)
            {
                result = JsonConvert.DeserializeObject<CareRecipientSearchResponse>(response);

                if (result != null)
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
            }
            else
            {
                var message = JsonConvert.DeserializeObject<MessageResponse>(response);
                errors.Add(new AppException(message.Message, message.Message));
            }
        }
        else
        {
            result = null;
        }

        return await Task.FromResult((result, errors));
    }

    public async Task<(List<QueryEntryEventsResponse> response, List<AppException> errors)> QueryResidentialEntryEvents(
            string? careRecipientId,
            string? externalReferenceId,
            string? entryDateFrom,
            string? entryDateTo,
            int limit,
            string? page,
            string? sort)
    {
        var result = new List<QueryEntryEventsResponse>();
        var errors = new List<AppException>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var (success, response) = await httpUtil.executeQueryEntryEvents(
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

            if (success)
            {
                result = JsonConvert.DeserializeObject<List<QueryEntryEventsResponse>>(response);
            }
            else
            {
                var message = JsonConvert.DeserializeObject<MessageResponse>(response);
                errors.Add(new AppException(message.Message, message.Message));
            }
        }

        return await Task.FromResult((result, errors));
    }

    public async Task<(List<QueryEntryEventsResponse> response, List<AppException> errors)> QueryHomeEntryEvents(
            string? careRecipientId,
            string? externalReferenceId,
            string? entryDateFrom,
            string? entryDateTo,
            int limit,
            string? page,
            string? sort)
    {
        var result = new List<QueryEntryEventsResponse>();
        var errors = new List<AppException>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var (success, response) = await httpUtil.executeQueryEntryEvents(
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

            if (success)
            {
                result = JsonConvert.DeserializeObject<List<QueryEntryEventsResponse>>(response);
            }
            else
            {
                var message = JsonConvert.DeserializeObject<MessageResponse>(response);
                errors.Add(new AppException(message.Message, message.Message));
            }
        }

        return await Task.FromResult((result, errors));
    }

    public async Task<(GetEntryEventDetailsResponse response, List<AppException> errors)> GetEntryEventDetails(string eventId)
    {
        var result = new GetEntryEventDetailsResponse();
        var errors = new List<AppException>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var (success, response) = await httpUtil.executeGetEntryEventDetails(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId,
            eventId);

            if (success)
            {
                result = JsonConvert.DeserializeObject<GetEntryEventDetailsResponse>(response);
            }
            else
            {
                var message = JsonConvert.DeserializeObject<MessageResponse>(response);
                errors.Add(new AppException(message.Message, message.Message));
            }
        }

        return await Task.FromResult((result, errors));
    }

    public async Task<(CreateEntryEventResponse response, List<AppException> errors)> CreateEntryEvent(string tempAccessKey, CreateEntryEventRequest request)
    {
        var result = new CreateEntryEventResponse();
        var errors = new List<AppException>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var (success, response, etag) = await httpUtil.executeCreateEntryEvent(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            request,
            tempAccessKey,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId);

            if (success)
            {
                result = JsonConvert.DeserializeObject<CreateEntryEventResponse>(response);

                // save the event to the database
                await _eventRepository.AddRecordAsync(new Event() { CareRecipientId = request.CareRecipientId, EventId = result.EventId, Etag = etag });
            }
            else
            {
                var message = JsonConvert.DeserializeObject<MessageResponse>(response);
                errors.Add(new AppException(message.Message, message.Message));
            }
        }

        return await Task.FromResult((result, errors));
    }

    public async Task<(bool response, List<AppException> errors)> UpdateEntryEvent(string eventId, UpdateEntryEventRequest request)
    {
        var isSuccessful = false;
        var errors = new List<AppException>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            // get the etag
            var record = await _eventRepository.GetByEventIdAsync(eventId);
            if (record != null)
            {
                var (success, etag, response) = await httpUtil.executeUpdateEntryEvent(
                            _servicesAustraliaDeviceConfig.OrganisationRA,
                            _servicesAustraliaDeviceConfig.DeviceName,
                            _servicesAustraliaDeviceConfig.ProductId,
                            accessToken,
                            _servicesAustraliaDeviceConfig.BaseUrl,
                            eventId,
                            record.Etag,
                            request,
                            _servicesAustraliaDeviceConfig.ServiceNapsId,
                            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId);

                // update event etag in the database
                if (success)
                {
                    record.Etag = etag;
                    await _eventRepository.UpdateRecordAsync(record);
                    isSuccessful = true;
                }
                else
                {
                    var message = JsonConvert.DeserializeObject<MessageResponse>(response);
                    errors.Add(new AppException(message.Message, message.Message));
                }
            }
        }

        return await Task.FromResult((isSuccessful, errors));
    }

    public async Task<(DeleteEntryEventResponse response, List<AppException> errors)> DeleteEntryEvent(string eventId)
    {
        var result = new DeleteEntryEventResponse();
        var errors = new List<AppException>();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            // get the etag
            var record = await _eventRepository.GetByEventIdAsync(eventId);
            if (record != null)
            {
                var (success, response) = await httpUtil.executeDeleteEntryEvent(
                _servicesAustraliaDeviceConfig.OrganisationRA,
                _servicesAustraliaDeviceConfig.DeviceName,
                _servicesAustraliaDeviceConfig.ProductId,
                accessToken,
                _servicesAustraliaDeviceConfig.BaseUrl,
                _servicesAustraliaDeviceConfig.ServiceNapsId,
                _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId,
                eventId,
                record.Etag);

                if (success)
                {
                    result = JsonConvert.DeserializeObject<DeleteEntryEventResponse>(response);

                    // delete the event in the database
                    await _eventRepository.DeleteByEventIdAsync(eventId);
                    await _eventRepository.SaveChangesAsync();
                }
                else
                {
                    var message = JsonConvert.DeserializeObject<MessageResponse>(response);
                    errors.Add(new AppException(message.Message, message.Message));
                }
            }
        }

        return await Task.FromResult((result, errors));
    }

    public async Task<(List<EntryEventHistoryResponse> response, List<AppException> errors)> EntryEventHistory(string eventId)
    {
        var result = new List<EntryEventHistoryResponse>();
        var errors = new List<AppException >();

        var (log, accessToken) = _authenticationService.GetAccessToken(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var (success, response) = await httpUtil.executeEntryEventHistory(
            _servicesAustraliaDeviceConfig.OrganisationRA,
            _servicesAustraliaDeviceConfig.DeviceName,
            _servicesAustraliaDeviceConfig.ProductId,
            accessToken,
            _servicesAustraliaDeviceConfig.BaseUrl,
            _servicesAustraliaDeviceConfig.ServiceNapsId,
            _servicesAustraliaDeviceConfig.AgedCareResidentialServiceId,
            eventId);

            if (success)
            {
                result = JsonConvert.DeserializeObject<List<EntryEventHistoryResponse>>(response);
            }
            else
            {
                var message = JsonConvert.DeserializeObject<MessageResponse>(response);
                errors.Add(new AppException(message.Message, message.Message));
            }
        }

        return await Task.FromResult((result, errors));
    }
}