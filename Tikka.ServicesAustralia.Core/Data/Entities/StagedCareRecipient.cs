using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Entities;

public class StagedCareRecipient
{
    public Guid Id { get; set; }

    public required string CareRecipientId { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? BirthDate { get; set; }

    public string? Gender { get; set; }

    public required string TempAccessKey { get; set; }

    public required string TempAccessExpiry { get; set; }
}