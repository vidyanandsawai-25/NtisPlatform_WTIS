using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Mappings.WTIS;

/// <summary>
/// AutoMapper profile for Connection Category
/// </summary>
public class ConnectionCategoryMappingProfile : Profile
{
    public ConnectionCategoryMappingProfile()
    {
        // Entity to DTO
        CreateMap<ConnectionCategoryMasterEntity, ConnectionCategoryDto>();

        // Create DTO to Entity
        CreateMap<CreateConnectionCategoryDto, ConnectionCategoryMasterEntity>();

        // Update DTO to Entity
        CreateMap<UpdateConnectionCategoryDto, ConnectionCategoryMasterEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
