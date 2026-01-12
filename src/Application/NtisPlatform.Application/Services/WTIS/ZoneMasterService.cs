using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;
using NtisPlatform.Application.Services;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces;

namespace NtisPlatform.Application.Services.WTIS;

public class ZoneMasterService 
    : BaseCommonCrudService<ZoneMasterEntity, ZoneMasterDto, CreateZoneMasterDto, UpdateZoneMasterDto, ZoneMasterQueryParameters, int>, 
      IZoneMasterService
{
    public ZoneMasterService(
        IRepository<ZoneMasterEntity, int> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(repository, unitOfWork, mapper)
    {
    }
}
