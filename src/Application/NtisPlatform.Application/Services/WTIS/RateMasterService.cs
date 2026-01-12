using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NtisPlatform.Application.DTOs.Queries;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;
using NtisPlatform.Application.Services;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces;
using NtisPlatform.Core.Interfaces.WTIS;
using NtisPlatform.Application.Models;

namespace NtisPlatform.Application.Services.WTIS;

public class RateMasterService 
    : BaseCommonCrudService<RateMasterEntity, RateMasterDto, CreateRateMasterDto, UpdateRateMasterDto, RateMasterQueryParameters, int>,
      IRateMasterService
{
    private readonly IRateMasterRepository _rateMasterRepository;

    public RateMasterService(
        IRepository<RateMasterEntity, int> repository,
        IRateMasterRepository rateMasterRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(repository, unitOfWork, mapper)
    {
        _rateMasterRepository = rateMasterRepository;
    }

    public override async Task<PagedResult<RateMasterDto>> GetAllAsync(
        RateMasterQueryParameters queryParams,
        CancellationToken cancellationToken = default)
    {
        var allData = await _rateMasterRepository.GetAllWithJoinsAsync(cancellationToken);
        
        var filtered = allData.AsQueryable();
        
        if (queryParams.ZoneID.HasValue)
            filtered = filtered.Where(r => r.ZoneID == queryParams.ZoneID);
        
        if (queryParams.WardID.HasValue)
            filtered = filtered.Where(r => r.WardID == queryParams.WardID);
        
        if (queryParams.TapSizeID.HasValue)
            filtered = filtered.Where(r => r.TapSizeID == queryParams.TapSizeID);
        
        if (queryParams.ConnectionTypeID.HasValue)
            filtered = filtered.Where(r => r.ConnectionTypeID == queryParams.ConnectionTypeID);
        
        if (queryParams.ConnectionCategoryID.HasValue)
            filtered = filtered.Where(r => r.ConnectionCategoryID == queryParams.ConnectionCategoryID);
        
        if (queryParams.Year.HasValue)
            filtered = filtered.Where(r => r.Year == queryParams.Year);

        if (queryParams.IsActive.HasValue)
            filtered = filtered.Where(r => r.IsActive == queryParams.IsActive);

        var totalCount = filtered.Count();
        
        if (!string.IsNullOrEmpty(queryParams.SortBy))
        {
            filtered = queryParams.SortBy.ToLower() switch
            {
                "year" => queryParams.SortOrder?.ToLower() == "desc" 
                    ? filtered.OrderByDescending(r => r.Year)
                    : filtered.OrderBy(r => r.Year),
                "zonename" => queryParams.SortOrder?.ToLower() == "desc"
                    ? filtered.OrderByDescending(r => r.ZoneName)
                    : filtered.OrderBy(r => r.ZoneName),
                "wardname" => queryParams.SortOrder?.ToLower() == "desc"
                    ? filtered.OrderByDescending(r => r.WardName)
                    : filtered.OrderBy(r => r.WardName),
                _ => filtered.OrderByDescending(r => r.Year)
            };
        }
        else
        {
            filtered = filtered.OrderByDescending(r => r.Year)
                .ThenBy(r => r.ZoneName)
                .ThenBy(r => r.WardName);
        }

        var items = filtered
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToList();

        var dtos = _mapper.Map<IEnumerable<RateMasterDto>>(items);

        return new PagedResult<RateMasterDto>(
            dtos,
            totalCount,
            queryParams.PageNumber,
            queryParams.PageSize
        );
    }

    public override async Task<RateMasterDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _rateMasterRepository.GetWithJoinsAsync(id, cancellationToken);
        return _mapper.Map<RateMasterDto>(result);
    }

    public override async Task<RateMasterDto> CreateAsync(CreateRateMasterDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<RateMasterEntity>(createDto);
        entity.CreatedDate = DateTime.Now;

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _rateMasterRepository.GetWithJoinsAsync(entity.RateID, cancellationToken);
        
        if (result == null)
        {
            return _mapper.Map<RateMasterDto>(entity);
        }
        
        return _mapper.Map<RateMasterDto>(result);
    }

    public override async Task<RateMasterDto?> UpdateAsync(int id, UpdateRateMasterDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTime.Now;

        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _rateMasterRepository.GetWithJoinsAsync(id, cancellationToken);
        return result == null ? _mapper.Map<RateMasterDto>(entity) : _mapper.Map<RateMasterDto>(result);
    }
}
