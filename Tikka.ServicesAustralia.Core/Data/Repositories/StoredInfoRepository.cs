using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Repositories;

public class StoredInfoRepository : IStoredInfoRepository
{
    private readonly TikkaDbContext _tikkaDbContext;

    public StoredInfoRepository(TikkaDbContext tikkaDbContext)
    {
        _tikkaDbContext = tikkaDbContext;
    }

    public async Task<StoredInfo> AddRecordAsync(StoredInfo record)
    {
        _tikkaDbContext.StoredInfos.Add(record);
        await _tikkaDbContext.SaveChangesAsync();
        return record;
    }

    public async Task<StoredInfo?> GetByIdAsync(Guid id)
    {
        return await _tikkaDbContext.StoredInfos
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<StoredInfo?> GetByCodeAsync(StoredInfoCode code)
    {
        return await _tikkaDbContext.StoredInfos
            .FirstOrDefaultAsync(u => u.Code == code);
    }

    public async Task<StoredInfo?> UpsertByCodeAsync(StoredInfoCode code, string value)
    {
        var record = await _tikkaDbContext.StoredInfos
            .FirstOrDefaultAsync(u => u.Code == code);

        if (record == null)
        {
            record = new StoredInfo
            {
                Id = Guid.NewGuid(),
                CreatedOnUtc = DateTime.UtcNow,
                Code = code,
                Value = value
            };
            _tikkaDbContext.StoredInfos.Add(record);
        }
        else
        {
            record.Value = value;
            record.LastModifiedOnUtc = DateTime.UtcNow;
            _tikkaDbContext.StoredInfos.Update(record);
        }

        await _tikkaDbContext.SaveChangesAsync();
        return await Task.FromResult(record);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _tikkaDbContext.SaveChangesAsync() > 0;
    }
}