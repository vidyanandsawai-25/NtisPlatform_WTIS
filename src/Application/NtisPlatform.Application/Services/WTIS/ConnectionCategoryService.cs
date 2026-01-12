using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;
using NtisPlatform.Application.Services;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces;

namespace NtisPlatform.Application.Services.WTIS;

public class ConnectionCategoryService 
    : BaseCommonCrudService<ConnectionCategoryMasterEntity, ConnectionCategoryDto, CreateConnectionCategoryDto, UpdateConnectionCategoryDto, ConnectionCategoryQueryParameters, int>, 
      IConnectionCategoryService
{
    public ConnectionCategoryService(
        IRepository<ConnectionCategoryMasterEntity, int> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(repository, unitOfWork, mapper)
    {
    }
}
