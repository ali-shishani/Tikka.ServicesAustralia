using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Repositories;

public class StagedCareRecipientRepository : IStagedCareRecipientRepository
{
    private readonly TikkaDbContext _tikkaDbContext;

    public StagedCareRecipientRepository(TikkaDbContext tikkaDbContext)
    {
        _tikkaDbContext = tikkaDbContext;
    }

    public async Task<StagedCareRecipient> AddRecordAsync(StagedCareRecipient record)
    {
        _tikkaDbContext.StagedCareRecipients.Add(record);
        await _tikkaDbContext.SaveChangesAsync();
        return record;
    }

    public async Task<StagedCareRecipient?> GetByIdAsync(Guid id)
    {
        return await _tikkaDbContext.StagedCareRecipients
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _tikkaDbContext.SaveChangesAsync() > 0;
    }
}