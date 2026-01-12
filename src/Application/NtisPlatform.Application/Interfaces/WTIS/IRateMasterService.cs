using NtisPlatform.Application.Models;
using NtisPlatform.Application.DTOs.WTIS;

namespace NtisPlatform.Application.Interfaces.WTIS;

/// <summary>
/// Rate Master service interface - All operations return joined data
/// </summary>
public interface IRateMasterService
{
    /// <summary>
    /// Get all rate masters with joined master table names (default behavior)
    /// </summary>
    Task<PagedResult<RateMasterDto>> GetAllAsync(RateMasterQueryParameters queryParams, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get rate master by ID with joined master table names (default behavior)
    /// </summary>
    Task<RateMasterDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new rate master (uses IDs, returns joined data)
    /// </summary>
    Task<RateMasterDto> CreateAsync(CreateRateMasterDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update rate master (uses IDs, returns joined data)
    /// </summary>
    Task<RateMasterDto?> UpdateAsync(int id, UpdateRateMasterDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete rate master
    /// </summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
