using System.Security.Claims;
using Tikka.ServicesAustralia.Core.Models;
using Tikka.ServicesAustralia.Models.Requests;
using Tikka.ServicesAustralia.Models.Responses;

namespace Tikka.ServicesAustralia.Services;

public interface IDataService
{
    Task<(CareRecipientSearchResponse response, List<AppException> errors)> CareRecipientSearch(string careRecipientId, string firstName, string middleName, string lastName, string gender, string birthDate, string postCode, string State);
    Task<(List<QueryEntryEventsResponse> response, List<AppException> errors)> QueryResidentialEntryEvents(string? careRecipientId, string? externalReferenceId, string? entryDateFrom, string? entryDateTo, int limit, string? page, string? sort);
    Task<(List<QueryEntryEventsResponse> response, List<AppException> errors)> QueryHomeEntryEvents(string? careRecipientId, string? externalReferenceId, string? entryDateFrom, string? entryDateTo, int limit, string? page, string? sort);
    Task<(GetEntryEventDetailsResponse response, List<AppException> errors)> GetEntryEventDetails(string eventId);
    Task<(CreateEntryEventResponse response, List<AppException> errors)> CreateEntryEvent(string tempAccessKey, CreateEntryEventRequest request);
    Task<(bool response, List<AppException> errors)> UpdateEntryEvent(string eventId, UpdateEntryEventRequest request);
    Task<(DeleteEntryEventResponse response, List<AppException> errors)> DeleteEntryEvent(string eventId);
    Task<(List<EntryEventHistoryResponse> response, List<AppException> errors)> EntryEventHistory(string eventId);
}