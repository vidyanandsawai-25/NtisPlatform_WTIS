using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Core.Interfaces.WTIS;

/// <summary>
/// Repository interface for Rate Master with joined master data
/// </summary>
public interface IRateMasterRepository : IRepository<RateMasterEntity, int>
{
    /// <summary>
    /// Get rate master with joined master table data
    /// </summary>
    Task<RateMasterWithJoins?> GetWithJoinsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all rate masters with joined master table data
    /// </summary>
    Task<IEnumerable<RateMasterWithJoins>> GetAllWithJoinsAsync(CancellationToken cancellationToken = default);
}
