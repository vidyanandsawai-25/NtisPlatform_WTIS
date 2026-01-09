using Microsoft.EntityFrameworkCore;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces.WTIS;
using NtisPlatform.Infrastructure.Data;

namespace NtisPlatform.Infrastructure.Repositories.WTIS;

/// <summary>
/// Repository for Consumer Account with LINQ queries and master table joins (optimized for read operations)
/// </summary>
public class ConsumerAccountRepository : Repository<ConsumerAccountEntity, int>, IConsumerAccountRepository
{
    #region Constructor

    public ConsumerAccountRepository(ApplicationDbContext context) : base(context)
    {
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Universal find by any identifier with master table data (single result)
    /// Supports: ConsumerNumber, Mobile, Name, Email, ID, or Ward-Property-Partition pattern
    /// </summary>
    public async Task<ConsumerAccountWithMasterData?> FindConsumerAsync(
        string searchValue,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
            return null;

        searchValue = searchValue.Trim();

        // Pattern detection: Ward-Property-Partition (e.g., "1-PROP011-FLAT-101")
        if (searchValue.Contains('-'))
        {
            var result = await FindByPatternAsync(searchValue, cancellationToken);
            if (result != null) return result;
        }

        // Direct field search (Consumer#, Mobile, Name, Email, etc.)
        return await FindByDirectFieldAsync(searchValue, cancellationToken);
    }

    /// <summary>
    /// Search by multiple filters with master table data (list results with pagination support)
    /// </summary>
    public async Task<IEnumerable<ConsumerAccountWithMasterData>> SearchByMultipleColumnsAsync(
        string? consumerNumber = null,
        string? oldConsumerNumber = null,
        string? mobileNumber = null,
        string? wardNo = null,
        string? propertyNumber = null,
        string? zoneNo = null,
        string? consumerName = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        // Build dynamic LINQ query with filters
        var query = BuildSearchQuery(
            consumerNumber, oldConsumerNumber, mobileNumber,
            wardNo, propertyNumber, zoneNo, consumerName, isActive);

        // Execute optimized query with projections
        return await query.ToListAsync(cancellationToken);
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Find by Ward-Property-Partition pattern (hierarchical search)
    /// </summary>
    private async Task<ConsumerAccountWithMasterData?> FindByPatternAsync(
        string pattern,
        CancellationToken cancellationToken)
    {
        var parts = pattern.Split('-', StringSplitOptions.TrimEntries);
        if (parts.Length < 2) return null;

        var wardNo = parts[0];
        var propertyNumber = parts.Length > 1 ? parts[1] : null;
        var partitionNumber = parts.Length > 2 ? parts[2] : null;

        // Build query with pattern matching
        var query = GetBaseQueryWithJoins()
            .Where(ca => ca.IsActive == true && ca.WardNo == wardNo);

        // Add property filter if provided
        if (!string.IsNullOrWhiteSpace(propertyNumber))
            query = query.Where(ca => ca.PropertyNumber == propertyNumber);

        // Add partition filter if provided
        if (!string.IsNullOrWhiteSpace(partitionNumber))
            query = query.Where(ca => ca.PartitionNumber == partitionNumber);

        // Execute query with projection
        return await query
            .OrderBy(ca => ca.ConsumerID)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Find by direct field match (Consumer#, Mobile, Name, Email, ID)
    /// </summary>
    private async Task<ConsumerAccountWithMasterData?> FindByDirectFieldAsync(
        string searchValue,
        CancellationToken cancellationToken)
    {
        // Try parsing as ID
        var isNumeric = int.TryParse(searchValue, out var consumerId);

        // Query multiple indexed fields with OR condition
        var query = GetBaseQueryWithJoins()
            .Where(ca => ca.IsActive == true &&
                (ca.ConsumerNumber == searchValue ||
                 ca.MobileNumber == searchValue ||
                 ca.ConsumerName == searchValue ||
                 ca.ConsumerNameEnglish == searchValue ||
                 ca.OldConsumerNumber == searchValue ||
                 ca.EmailID == searchValue ||
                 ca.PropertyNumber == searchValue ||
                 ca.PartitionNumber == searchValue ||
                 (isNumeric && ca.ConsumerID == consumerId)));

        // Execute query with projection
        return await query
            .OrderBy(ca => ca.ConsumerID)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Build dynamic LINQ query with filters (type-safe and parameterized)
    /// </summary>
    private IQueryable<ConsumerAccountWithMasterData> BuildSearchQuery(
        string? consumerNumber, string? oldConsumerNumber, string? mobileNumber,
        string? wardNo, string? propertyNumber, string? zoneNo,
        string? consumerName, bool? isActive)
    {
        var query = GetBaseQueryWithJoins();

        // Active status filter (default to active only)
        query = isActive.HasValue
            ? query.Where(ca => ca.IsActive == isActive.Value)
            : query.Where(ca => ca.IsActive == true);

        // Add filters only if provided (optimizes query performance)
        if (!string.IsNullOrWhiteSpace(consumerNumber))
            query = query.Where(ca => ca.ConsumerNumber.Contains(consumerNumber));

        if (!string.IsNullOrWhiteSpace(oldConsumerNumber))
            query = query.Where(ca => ca.OldConsumerNumber != null && ca.OldConsumerNumber.Contains(oldConsumerNumber));

        if (!string.IsNullOrWhiteSpace(mobileNumber))
            query = query.Where(ca => ca.MobileNumber != null && ca.MobileNumber.Contains(mobileNumber));

        if (!string.IsNullOrWhiteSpace(wardNo))
            query = query.Where(ca => ca.WardNo == wardNo);

        if (!string.IsNullOrWhiteSpace(zoneNo))
            query = query.Where(ca => ca.ZoneNo == zoneNo);

        // Property filter with pattern support
        if (!string.IsNullOrWhiteSpace(propertyNumber))
        {
            // If WardNo already provided and property doesn't contain pattern, use exact match
            if (!string.IsNullOrWhiteSpace(wardNo) && !propertyNumber.Contains('-'))
            {
                query = query.Where(ca => ca.PropertyNumber == propertyNumber);
            }
            // Pattern parsing for Ward-Property-Partition
            else if (propertyNumber.Contains('-'))
            {
                var parts = propertyNumber.Split('-', StringSplitOptions.TrimEntries);
                if (parts.Length >= 2)
                {
                    var patternWard = parts[0];
                    var patternProperty = parts[1];

                    // Add WardNo filter if not already added
                    if (string.IsNullOrWhiteSpace(wardNo))
                        query = query.Where(ca => ca.WardNo == patternWard);

                    // Add PropertyNumber filter
                    if (!string.IsNullOrWhiteSpace(patternProperty))
                        query = query.Where(ca => ca.PropertyNumber == patternProperty);

                    // Add PartitionNumber filter if provided
                    if (parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2]))
                    {
                        var patternPartition = parts[2];
                        query = query.Where(ca => ca.PartitionNumber == patternPartition);
                    }
                }
            }
            else
            {
                // Partial match
                query = query.Where(ca => ca.PropertyNumber != null && ca.PropertyNumber.Contains(propertyNumber));
            }
        }

        // Name filter (searches both Hindi and English)
        if (!string.IsNullOrWhiteSpace(consumerName))
            query = query.Where(ca =>
                ca.ConsumerName.Contains(consumerName) ||
                (ca.ConsumerNameEnglish != null && ca.ConsumerNameEnglish.Contains(consumerName)));

        // Sort by indexed field
        return query.OrderBy(ca => ca.ConsumerID);
    }

    /// <summary>
    /// Get base query with LEFT JOINs to master tables (single optimized query)
    /// </summary>
    private IQueryable<ConsumerAccountWithMasterData> GetBaseQueryWithJoins()
    {
        return from ca in _context.Set<ConsumerAccountEntity>().AsNoTracking()
               // LEFT JOIN ConnectionTypeMaster
               join ct in _context.Set<ConnectionTypeMasterEntity>().AsNoTracking()
                   on ca.ConnectionTypeID equals ct.ConnectionTypeID into ctJoin
               from ct in ctJoin.DefaultIfEmpty()
               // LEFT JOIN ConnectionCategoryMaster
               join cc in _context.Set<ConnectionCategoryMasterEntity>().AsNoTracking()
                   on ca.CategoryID equals cc.CategoryID into ccJoin
               from cc in ccJoin.DefaultIfEmpty()
               // LEFT JOIN PipeSizeMaster
               join ps in _context.Set<PipeSizeMasterEntity>().AsNoTracking()
                   on ca.PipeSizeID equals ps.PipeSizeID into psJoin
               from ps in psJoin.DefaultIfEmpty()
               // Project to result with master data
               select new ConsumerAccountWithMasterData
               {
                   // Identity
                   ConsumerID = ca.ConsumerID,
                   ConsumerNumber = ca.ConsumerNumber,
                   OldConsumerNumber = ca.OldConsumerNumber,

                   // Location hierarchy
                   ZoneNo = ca.ZoneNo,
                   WardNo = ca.WardNo,
                   PropertyNumber = ca.PropertyNumber,
                   PartitionNumber = ca.PartitionNumber,

                   // Consumer details
                   ConsumerName = ca.ConsumerName,
                   ConsumerNameEnglish = ca.ConsumerNameEnglish,
                   MobileNumber = ca.MobileNumber,
                   EmailID = ca.EmailID,
                   Address = ca.Address,
                   AddressEnglish = ca.AddressEnglish,

                   // Connection details (IDs)
                   ConnectionTypeID = ca.ConnectionTypeID,
                   CategoryID = ca.CategoryID,
                   PipeSizeID = ca.PipeSizeID,

                   // Master table names (from LEFT JOINs)
                   ConnectionTypeName = ct != null ? ct.ConnectionTypeName : null,
                   CategoryName = cc != null ? cc.CategoryName : null,
                   PipeSize = ps != null ? ps.SizeName : null,

                   // Additional info
                   ConnectionDate = ca.ConnectionDate,
                   IsActive = ca.IsActive,
                   Remark = ca.Remark,
                   CreatedDate = ca.CreatedDate,
                   UpdatedDate = ca.UpdatedDate
               };
    }

    #endregion
}
