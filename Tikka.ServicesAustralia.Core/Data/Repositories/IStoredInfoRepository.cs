using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Repositories;

public interface IStoredInfoRepository
{
    Task<StoredInfo> AddRecordAsync(StoredInfo record);
    Task<StoredInfo?> GetByIdAsync(Guid id);
    Task<StoredInfo?> GetByCodeAsync(StoredInfoCode code);
    Task<StoredInfo?> UpsertByCodeAsync(StoredInfoCode code, string value);
    Task<bool> SaveChangesAsync();
}