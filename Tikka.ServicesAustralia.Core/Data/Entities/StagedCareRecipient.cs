using System.ComponentModel.DataAnnotations;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Entities;

public class StagedCareRecipient
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public required string CareRecipientId { get; set; }

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? MiddleName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(10)]
    public string? BirthDate { get; set; }

    [MaxLength(10)]
    public string? Gender { get; set; }

    [MaxLength(100)]
    public required string TempAccessKey { get; set; }

    [MaxLength(30)]
    public required string TempAccessExpiry { get; set; }
}