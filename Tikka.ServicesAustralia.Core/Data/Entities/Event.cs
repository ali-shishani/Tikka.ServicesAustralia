using System.ComponentModel.DataAnnotations;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Entities;

public class Event
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public required string EventId { get; set; }

    [MaxLength(100)]
    public required string CareRecipientId { get; set; }

    [MaxLength(100)]
    public required string Etag { get; set; }
}