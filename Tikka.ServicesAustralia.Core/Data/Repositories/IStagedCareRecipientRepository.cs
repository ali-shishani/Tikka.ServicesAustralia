using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Data.Entities.Enums;

namespace Tikka.ServicesAustralia.Core.Data.Repositories;

public interface IStagedCareRecipientRepository
{
    Task<StagedCareRecipient> AddRecordAsync(StagedCareRecipient record);
    Task<StagedCareRecipient?> GetByIdAsync(Guid id);
    Task<bool> SaveChangesAsync();
}