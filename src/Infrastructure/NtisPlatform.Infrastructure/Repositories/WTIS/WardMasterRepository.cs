using Microsoft.EntityFrameworkCore;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces.WTIS;
using NtisPlatform.Infrastructure.Data;

namespace NtisPlatform.Infrastructure.Repositories.WTIS;

/// <summary>
/// Ward Master repository with Zone information support
/// </summary>
public class WardMasterRepository : Repository<WardMasterEntity, int>, IWardMasterRepository
{
    public WardMasterRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<WardMasterWithZone>> GetAllWithZoneAsync(CancellationToken cancellationToken = default)
    {
        return await GetBaseQueryWithZone().ToListAsync(cancellationToken);
    }

    public async Task<WardMasterWithZone?> GetByIdWithZoneAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetBaseQueryWithZone()
            .Where(w => w.WardID == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private IQueryable<WardMasterWithZone> GetBaseQueryWithZone()
    {
        return from w in _context.Set<WardMasterEntity>().AsNoTracking()
               join z in _context.Set<ZoneMasterEntity>().AsNoTracking()
                   on w.ZoneID equals z.ZoneID into zJoin
               from z in zJoin.DefaultIfEmpty()
               select new WardMasterWithZone
               {
                   WardID = w.WardID,
                   WardName = w.WardName,
                   WardCode = w.WardCode,
                   ZoneID = w.ZoneID,
                   ZoneName = z != null ? z.ZoneName : null,
                   IsActive = w.IsActive,
                   CreatedBy = w.CreatedBy,
                   CreatedDate = w.CreatedDate,
                   UpdatedBy = w.UpdatedBy,
                   UpdatedDate = w.UpdatedDate
               };
    }
}
