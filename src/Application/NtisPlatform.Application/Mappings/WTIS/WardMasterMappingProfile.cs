using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Core.Entities.WTIS;
using NtisPlatform.Core.Interfaces.WTIS;

namespace NtisPlatform.Application.Mappings.WTIS;

/// <summary>
/// AutoMapper profile for Ward Master
/// </summary>
public class WardMasterMappingProfile : Profile
{
    public WardMasterMappingProfile()
    {
        // Entity to DTO
        CreateMap<WardMasterEntity, WardMasterDto>();

        // WithZone to DTO
        CreateMap<WardMasterWithZone, WardMasterDto>();

        // Create DTO to Entity
        CreateMap<CreateWardMasterDto, WardMasterEntity>();

        // Update DTO to Entity
        CreateMap<UpdateWardMasterDto, WardMasterEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
