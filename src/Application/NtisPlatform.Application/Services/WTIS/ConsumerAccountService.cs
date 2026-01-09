using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;
using NtisPlatform.Core.Interfaces.WTIS;

namespace NtisPlatform.Application.Services.WTIS;

/// <summary>
/// Read-only service for Consumer Account search operations
/// </summary>
public class ConsumerAccountService : IConsumerAccountService
{
    #region Fields

    private readonly IConsumerAccountRepository _repository;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public ConsumerAccountService(IConsumerAccountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Universal search by any identifier (Consumer#, Mobile, Name, Pattern, etc.)
    /// </summary>
    public async Task<ConsumerAccountDto?> FindConsumerAsync(string searchValue, CancellationToken cancellationToken = default)
    {
        var result = await _repository.FindConsumerAsync(searchValue, cancellationToken);
        return result != null ? _mapper.Map<ConsumerAccountDto>(result) : null;
    }

    /// <summary>
    /// Search with multiple filters (returns all matching results without pagination)
    /// </summary>
    public async Task<IEnumerable<ConsumerAccountDto>> SearchConsumersAsync(
        ConsumerAccountSearchDto searchDto,
        CancellationToken cancellationToken = default)
    {
        // Get all filtered results from repository
        var results = await _repository.SearchByMultipleColumnsAsync(
            searchDto.ConsumerNumber,
            searchDto.OldConsumerNumber,
            searchDto.MobileNumber,
            searchDto.WardNo,
            searchDto.PropertyNumber,
            searchDto.ZoneNo,
            searchDto.ConsumerName,
            searchDto.IsActive,
            searchDto.EmailID,
            cancellationToken);

        // Map to DTOs and return all results
        return _mapper.Map<IEnumerable<ConsumerAccountDto>>(results);
    }

    #endregion
}
