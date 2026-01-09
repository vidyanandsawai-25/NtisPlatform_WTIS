using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Core.Interfaces.WTIS;

/// <summary>
/// Repository interface for Consumer Account (read-only operations with SQL JOINs)
/// </summary>
public interface IConsumerAccountRepository : IRepository<ConsumerAccountEntity, int>
{
    /// <summary>
    /// Universal find by any identifier with master table data (single result)
    /// Supports: Consumer#, Mobile, Name, Email, ID, or Ward-Property-Partition pattern
    /// </summary>
    Task<ConsumerAccountWithMasterData?> FindConsumerAsync(
        string searchValue,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search by multiple filters with master table data (list results)
    /// PropertyNumber supports Ward-Property-Partition pattern (e.g., "1-PROP011-FLAT-101")
    /// </summary>
    Task<IEnumerable<ConsumerAccountWithMasterData>> SearchByMultipleColumnsAsync(
        string? consumerNumber = null,
        string? oldConsumerNumber = null,
        string? mobileNumber = null,
        string? wardNo = null,
        string? propertyNumber = null,
        string? zoneNo = null,
        string? consumerName = null,
        bool? isActive = null,
        string? emailID = null,
        CancellationToken cancellationToken = default);
}
