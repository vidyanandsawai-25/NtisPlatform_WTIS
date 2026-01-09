using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Models;

namespace NtisPlatform.Application.Interfaces.WTIS;

/// <summary>
/// Consumer Account service interface (read-only operations)
/// </summary>
public interface IConsumerAccountService
{
    /// <summary>
    /// Get by ID (returns single result with master table data)
    /// </summary>
    Task<ConsumerAccountDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all with pagination and filtering (returns active consumers by default)
    /// </summary>
    Task<PagedResult<ConsumerAccountDto>> GetAllAsync(
        ConsumerAccountQueryParameters queryParameters,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Universal search by any identifier (Consumer#, Mobile, Name, Pattern, etc.)
    /// </summary>
    Task<ConsumerAccountDto?> FindConsumerAsync(
        string searchValue,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search with multiple filters and pagination (optimized for large datasets)
    /// </summary>
    Task<PagedResult<ConsumerAccountDto>> SearchConsumersAsync(
        ConsumerAccountSearchDto searchDto,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}
