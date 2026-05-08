using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Repositories;

public class EventRepository : IEventRepository
{
    private readonly TikkaDbContext _tikkaDbContext;

    public EventRepository(TikkaDbContext tikkaDbContext)
    {
        _tikkaDbContext = tikkaDbContext;
    }

    public async Task<Event> AddRecordAsync(Event record)
    {
        _tikkaDbContext.Events.Add(record);
        await _tikkaDbContext.SaveChangesAsync();
        return record;
    }

    public async Task<Event> UpdateRecordAsync(Event record)
    {
        _tikkaDbContext.Events.Update(record);
        await _tikkaDbContext.SaveChangesAsync();
        return record;
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _tikkaDbContext.Events
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Event?> GetByEventIdAsync(string id)
    {
        return await _tikkaDbContext.Events
            .FirstOrDefaultAsync(u => u.EventId == id);
    }

    public async Task<Event?> GetByCareRicipientIdAsync(string id)
    {
        return await _tikkaDbContext.Events
            .FirstOrDefaultAsync(u => u.CareRecipientId == id);
    }


    public async Task<bool> DeleteByEventIdAsync(string id)
    {
        var record = await _tikkaDbContext.Events
            .FirstOrDefaultAsync(u => u.EventId == id);

        if (record != null)
        {
            _tikkaDbContext.Events.Remove(record);
            return true;
        }

        return false;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _tikkaDbContext.SaveChangesAsync() > 0;
    }
}