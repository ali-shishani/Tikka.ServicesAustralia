using System.Security.Claims;
using Tikka.ServicesAustralia.Models.Responses;

namespace Tikka.ServicesAustralia.Services;

public interface IDataService
{
    Task<CareRecipientSearchResponse> CareRecipientSearch(string careRecipientId, string firstName, string middleName, string lastName, string gender, string birthDate, string postCode, string State);
    Task<ResidentialCareEntryEventResponse> ResidentialCareEntry();
}