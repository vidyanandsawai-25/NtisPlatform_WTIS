using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Interfaces.WTIS;

/// <summary>
/// Connection Category service interface - Pure CRUD
/// </summary>
public interface IConnectionCategoryService 
    : ICommonCrudService<ConnectionCategoryMasterEntity, ConnectionCategoryDto, 
                        CreateConnectionCategoryDto, UpdateConnectionCategoryDto, 
                        ConnectionCategoryQueryParameters, int>
{
}
