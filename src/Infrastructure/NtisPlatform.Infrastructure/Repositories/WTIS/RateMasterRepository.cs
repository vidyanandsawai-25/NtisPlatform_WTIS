using Microsoft.EntityFrameworkCore;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces.WTIS;
using NtisPlatform.Infrastructure.Data;

namespace NtisPlatform.Infrastructure.Repositories.WTIS;

/// <summary>
/// Rate Master repository with joined master data support
/// </summary>
public class RateMasterRepository : Repository<RateMasterEntity, int>, IRateMasterRepository
{
    public RateMasterRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RateMasterWithJoins?> GetWithJoinsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetBaseQueryWithJoins()
            .Where(rm => rm.RateID == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<RateMasterWithJoins>> GetAllWithJoinsAsync(CancellationToken cancellationToken = default)
    {
        return await GetBaseQueryWithJoins()
            .OrderByDescending(rm => rm.Year)
            .ThenBy(rm => rm.ZoneName)
            .ThenBy(rm => rm.WardName)
            .ToListAsync(cancellationToken);
    }

    private IQueryable<RateMasterWithJoins> GetBaseQueryWithJoins()
    {
        return from rm in _context.Set<RateMasterEntity>().AsNoTracking()
               join z in _context.Set<ZoneMasterEntity>().AsNoTracking()
                   on rm.ZoneID equals z.ZoneID into zJoin
               from z in zJoin.DefaultIfEmpty()
               join w in _context.Set<WardMasterEntity>().AsNoTracking()
                   on rm.WardID equals w.WardID into wJoin
               from w in wJoin.DefaultIfEmpty()
               join ps in _context.Set<PipeSizeMasterEntity>().AsNoTracking()
                   on rm.TapSizeID equals ps.PipeSizeID into psJoin
               from ps in psJoin.DefaultIfEmpty()
               join ct in _context.Set<ConnectionTypeMasterEntity>().AsNoTracking()
                   on rm.ConnectionTypeID equals ct.ConnectionTypeID into ctJoin
               from ct in ctJoin.DefaultIfEmpty()
               join cc in _context.Set<ConnectionCategoryMasterEntity>().AsNoTracking()
                   on rm.ConnectionCategoryID equals cc.CategoryID into ccJoin
               from cc in ccJoin.DefaultIfEmpty()
               select new RateMasterWithJoins
               {
                   RateID = rm.RateID,
                   ZoneID = rm.ZoneID,
                   ZoneName = z != null ? z.ZoneName : null,
                   ZoneCode = z != null ? z.ZoneCode : null,
                   WardID = rm.WardID,
                   WardName = w != null ? w.WardName : null,
                   WardCode = w != null ? w.WardCode : null,
                   TapSizeID = rm.TapSizeID,
                   TapSize = ps != null ? ps.SizeName : null,
                   DiameterMM = ps != null ? ps.DiameterMM : (decimal?)null,
                   ConnectionTypeID = rm.ConnectionTypeID,
                   ConnectionTypeName = ct != null ? ct.ConnectionTypeName : null,
                   ConnectionCategoryID = rm.ConnectionCategoryID,
                   CategoryName = cc != null ? cc.CategoryName : null,
                   MinReading = rm.MinReading,
                   MaxReading = rm.MaxReading,
                   PerLiter = rm.PerLiter,
                   MinimumCharge = rm.MinimumCharge,
                   MeterOffPenalty = rm.MeterOffPenalty,
                   Rate = rm.Rate,
                   Year = rm.Year,
                   Remark = rm.Remark,
                   IsActive = rm.IsActive,
                   CreatedBy = rm.CreatedBy,
                   CreatedDate = rm.CreatedDate,
                   UpdatedBy = rm.UpdatedBy,
                   UpdatedDate = rm.UpdatedDate
               };
    }
}
