using Microsoft.EntityFrameworkCore;
using Tikka.ServicesAustralia.Core.Data.Entities;

namespace Tikka.ServicesAustralia.Core.Data;

public class TikkaDbContext : DbContext
{
    public TikkaDbContext(DbContextOptions<TikkaDbContext> options) : base(options)
    {
    }

    public virtual DbSet<StoredInfo> StoredInfos => Set<StoredInfo>();

    public virtual DbSet<StagedCareRecipient> StagedCareRecipients => Set<StagedCareRecipient>();

}