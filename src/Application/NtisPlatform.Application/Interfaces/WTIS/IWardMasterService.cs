using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Models;

namespace NtisPlatform.Application.Interfaces.WTIS;

/// <summary>
/// Ward Master service interface - Returns data with Zone information
/// </summary>
public interface IWardMasterService
{
    Task<PagedResult<WardMasterDto>> GetAllAsync(WardMasterQueryParameters queryParams, CancellationToken cancellationToken = default);
    Task<WardMasterDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<WardMasterDto> CreateAsync(CreateWardMasterDto createDto, CancellationToken cancellationToken = default);
    Task<WardMasterDto?> UpdateAsync(int id, UpdateWardMasterDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
