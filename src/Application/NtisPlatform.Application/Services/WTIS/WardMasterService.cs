using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;
using NtisPlatform.Application.Models;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces;
using NtisPlatform.Core.Interfaces.WTIS;
using Microsoft.EntityFrameworkCore;
using NtisPlatform.Application.Services;

namespace NtisPlatform.Application.Services.WTIS;

public class WardMasterService 
    : BaseCommonCrudService<WardMasterEntity, WardMasterDto, CreateWardMasterDto, UpdateWardMasterDto, WardMasterQueryParameters, int>,
      IWardMasterService
{
    private readonly IWardMasterRepository _wardMasterRepository;

    public WardMasterService(
        IRepository<WardMasterEntity, int> repository,
        IWardMasterRepository wardMasterRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(repository, unitOfWork, mapper)
    {
        _wardMasterRepository = wardMasterRepository;
    }

    public override async Task<PagedResult<WardMasterDto>> GetAllAsync(
        WardMasterQueryParameters queryParams,
        CancellationToken cancellationToken = default)
    {
        var allData = await _wardMasterRepository.GetAllWithZoneAsync(cancellationToken);
        
        var filtered = allData.AsQueryable();
        
        if (!string.IsNullOrEmpty(queryParams.WardName))
            filtered = filtered.Where(w => w.WardName.Contains(queryParams.WardName));
        
        if (queryParams.IsActive.HasValue)
            filtered = filtered.Where(w => w.IsActive == queryParams.IsActive);

        var totalCount = filtered.Count();
        
        if (!string.IsNullOrEmpty(queryParams.SortBy))
        {
            filtered = queryParams.SortBy.ToLower() switch
            {
                "wardname" => queryParams.SortOrder?.ToLower() == "desc"
                    ? filtered.OrderByDescending(w => w.WardName)
                    : filtered.OrderBy(w => w.WardName),
                "zonename" => queryParams.SortOrder?.ToLower() == "desc"
                    ? filtered.OrderByDescending(w => w.ZoneName)
                    : filtered.OrderBy(w => w.ZoneName),
                _ => filtered.OrderBy(w => w.WardName)
            };
        }
        else
        {
            filtered = filtered.OrderBy(w => w.WardName);
        }

        var items = filtered
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToList();

        var dtos = _mapper.Map<IEnumerable<WardMasterDto>>(items);

        return new PagedResult<WardMasterDto>(
            dtos,
            totalCount,
            queryParams.PageNumber,
            queryParams.PageSize
        );
    }

    public override async Task<WardMasterDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _wardMasterRepository.GetByIdWithZoneAsync(id, cancellationToken);
        return _mapper.Map<WardMasterDto>(result);
    }

    public override async Task<WardMasterDto> CreateAsync(CreateWardMasterDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WardMasterEntity>(createDto);
        entity.CreatedDate = DateTime.Now;

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _wardMasterRepository.GetByIdWithZoneAsync(entity.WardID, cancellationToken);
        
        if (result == null)
        {
            return _mapper.Map<WardMasterDto>(entity);
        }
        
        return _mapper.Map<WardMasterDto>(result);
    }

    public override async Task<WardMasterDto?> UpdateAsync(int id, UpdateWardMasterDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTime.Now;

        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _wardMasterRepository.GetByIdWithZoneAsync(id, cancellationToken);
        return result == null ? _mapper.Map<WardMasterDto>(entity) : _mapper.Map<WardMasterDto>(result);
    }
}
