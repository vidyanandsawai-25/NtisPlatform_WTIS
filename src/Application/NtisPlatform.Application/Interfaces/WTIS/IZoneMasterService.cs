using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Interfaces.WTIS;

/// <summary>
/// Zone Master service interface - Pure CRUD
/// </summary>
public interface IZoneMasterService 
    : ICommonCrudService<ZoneMasterEntity, ZoneMasterDto, 
                        CreateZoneMasterDto, UpdateZoneMasterDto, 
                        ZoneMasterQueryParameters, int>
{
}
