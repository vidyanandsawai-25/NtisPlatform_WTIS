using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Mappings.WTIS;

/// <summary>
/// AutoMapper profile for Zone Master
/// </summary>
public class ZoneMasterMappingProfile : Profile
{
    public ZoneMasterMappingProfile()
    {
        // Entity to DTO
        CreateMap<ZoneMasterEntity, ZoneMasterDto>();

        // Create DTO to Entity
        CreateMap<CreateZoneMasterDto, ZoneMasterEntity>();

        // Update DTO to Entity
        CreateMap<UpdateZoneMasterDto, ZoneMasterEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
