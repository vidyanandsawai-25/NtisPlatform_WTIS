using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;
using NtisPlatform.Application.Services;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces;

namespace NtisPlatform.Application.Services.WTIS;

/// <summary>
/// Pipe Size service - Pure CRUD using generic base
/// </summary>
public class PipeSizeService 
    : BaseCommonCrudService<PipeSizeMasterEntity, PipeSizeDto, 
                           CreatePipeSizeDto, UpdatePipeSizeDto, 
                           PipeSizeQueryParameters, int>,
      IPipeSizeService
{
    public PipeSizeService(
        IRepository<PipeSizeMasterEntity, int> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(repository, unitOfWork, mapper)
    {
    }
}
