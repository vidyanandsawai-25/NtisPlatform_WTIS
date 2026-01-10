using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;
using NtisPlatform.Application.Services;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces;

namespace NtisPlatform.Application.Services.WTIS;

/// <summary>
/// Connection Type service - Pure CRUD using generic base
/// </summary>
public class ConnectionTypeService 
    : BaseCommonCrudService<ConnectionTypeMasterEntity, ConnectionTypeDto, 
                           CreateConnectionTypeDto, UpdateConnectionTypeDto, 
                           ConnectionTypeQueryParameters, int>,
      IConnectionTypeService
{
    public ConnectionTypeService(
        IRepository<ConnectionTypeMasterEntity, int> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(repository, unitOfWork, mapper)
    {
    }
}
