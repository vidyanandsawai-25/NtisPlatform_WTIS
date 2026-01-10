using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Interfaces.WTIS;

/// <summary>
/// Pipe Size service interface - Pure CRUD
/// </summary>
public interface IPipeSizeService 
    : ICommonCrudService<PipeSizeMasterEntity, PipeSizeDto, 
                        CreatePipeSizeDto, UpdatePipeSizeDto, 
                        PipeSizeQueryParameters, int>
{
}
