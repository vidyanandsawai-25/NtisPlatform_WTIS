using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Interfaces.WTIS;

/// <summary>
/// Connection Type service interface - Pure CRUD
/// </summary>
public interface IConnectionTypeService 
    : ICommonCrudService<ConnectionTypeMasterEntity, ConnectionTypeDto, 
                        CreateConnectionTypeDto, UpdateConnectionTypeDto, 
                        ConnectionTypeQueryParameters, int>
{
}
