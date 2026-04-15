using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Entities;

public class StoredInfo
{
    public Guid Id { get; set; }
    public required DateTime CreatedOnUtc { get; set; }
    public DateTime? LastModifiedOnUtc { get; set; }
    public required StoredInfoCode Code { get; set; }
    public string? Value { get; set; }
}