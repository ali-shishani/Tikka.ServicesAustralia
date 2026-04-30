using System.Security.Claims;
using Tikka.ServicesAustralia.Models.Requests;
using Tikka.ServicesAustralia.Models.Responses;

namespace Tikka.ServicesAustralia.Services;

public interface IDataService
{
    Task<CareRecipientSearchResponse> CareRecipientSearch(string careRecipientId, string firstName, string middleName, string lastName, string gender, string birthDate, string postCode, string State);
    Task<List<QueryEntryEventsResponse>> QueryEntryEvents(string? careRecipientId, string? externalReferenceId, string? entryDateFrom, string? entryDateTo, int limit, string? page, string? sort);
    Task<GetEntryEventDetailsResponse> GetEntryEventDetails(string? eventId);
    Task<CreateEntryEventResponse> CreateEntryEvent(string tempAccessKey, CreateEntryEventRequest request);
    Task<UpdateEntryEventResponse> UpdateEntryEvent(string? eventId, UpdateEntryEventRequest request);
    Task<DeleteEntryEventResponse> DeleteEntryEvent(string? eventId);
}