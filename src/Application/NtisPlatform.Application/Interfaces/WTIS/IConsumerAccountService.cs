using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Models;

namespace NtisPlatform.Application.Interfaces.WTIS;

/// <summary>
/// Consumer Account service interface (read-only search operations)
/// </summary>
public interface IConsumerAccountService
{
    /// <summary>
    /// Universal search by any identifier (Consumer#, Mobile, Name, Pattern, etc.)
    /// Returns single result for exact matches
    /// </summary>
    Task<ConsumerAccountDto?> FindConsumerAsync(
        string searchValue,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search with multiple filters (returns all matching results without pagination)
    /// Used for hierarchical searches (Ward, Ward-Property)
    /// </summary>
    Task<IEnumerable<ConsumerAccountDto>> SearchConsumersAsync(
        ConsumerAccountSearchDto searchDto,
        CancellationToken cancellationToken = default);
}
