using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;
using NtisPlatform.Application.Models;
using NtisPlatform.Core.Interfaces.WTIS;

namespace NtisPlatform.Application.Services.WTIS;

/// <summary>
/// Read-only service for Consumer Account operations with master table data
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
    /// Get consumer by ID (returns single result with master table data)
    /// </summary>
    public async Task<ConsumerAccountDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.FindConsumerAsync(id.ToString(), cancellationToken);
        return result != null ? _mapper.Map<ConsumerAccountDto>(result) : null;
    }

    /// <summary>
    /// Get all active consumers with pagination and filtering
    /// </summary>
    public async Task<PagedResult<ConsumerAccountDto>> GetAllAsync(
        ConsumerAccountQueryParameters queryParameters,
        CancellationToken cancellationToken = default)
    {
        // Map query parameters to search DTO
        var searchDto = new ConsumerAccountSearchDto
        {
            ConsumerNumber = queryParameters.ConsumerNumber,
            OldConsumerNumber = queryParameters.OldConsumerNumber,
            ConsumerName = queryParameters.ConsumerName,
            MobileNumber = queryParameters.MobileNumber,
            WardNo = queryParameters.WardNo,
            ZoneNo = queryParameters.ZoneNo,
            PropertyNumber = queryParameters.PropertyNumber,
            IsActive = queryParameters.IsActive ?? true // Default to active only
        };

        return await SearchConsumersAsync(searchDto, queryParameters.PageNumber, queryParameters.PageSize, cancellationToken);
    }

    /// <summary>
    /// Universal search by any identifier (Consumer#, Mobile, Name, Pattern, etc.)
    /// </summary>
    public async Task<ConsumerAccountDto?> FindConsumerAsync(string searchValue, CancellationToken cancellationToken = default)
    {
        var result = await _repository.FindConsumerAsync(searchValue, cancellationToken);
        return result != null ? _mapper.Map<ConsumerAccountDto>(result) : null;
    }

    /// <summary>
    /// Search with multiple filters and pagination (optimized for large result sets)
    /// </summary>
    public async Task<PagedResult<ConsumerAccountDto>> SearchConsumersAsync(
        ConsumerAccountSearchDto searchDto,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // Get filtered results from repository
        var results = await _repository.SearchByMultipleColumnsAsync(
            searchDto.ConsumerNumber,
            searchDto.OldConsumerNumber,
            searchDto.MobileNumber,
            searchDto.WardNo,
            searchDto.PropertyNumber,
            searchDto.ZoneNo,
            searchDto.ConsumerName,
            searchDto.IsActive,
            cancellationToken);

        // Convert to list once (avoid multiple enumerations)
        var resultList = results as List<Core.Entities.WTIS.ConsumerAccountWithMasterData> ?? results.ToList();
        var totalCount = resultList.Count;

        // Apply pagination in-memory (results already filtered by DB)
        var pagedResults = resultList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Map to DTOs
        var dtos = _mapper.Map<List<ConsumerAccountDto>>(pagedResults);

        return new PagedResult<ConsumerAccountDto>(dtos, totalCount, pageNumber, pageSize);
    }

    #endregion
}
