using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Repositories;

public interface IEventRepository
{
    Task<Event> AddRecordAsync(Event record);
    Task<Event?> GetByIdAsync(Guid id);
    Task<Event?> GetByEventIdAsync(string id);
    Task<Event?> GetByCareRicipientIdAsync(string id);
    Task<bool> DeleteByEventIdAsync(string id);
    Task<bool> SaveChangesAsync();
}