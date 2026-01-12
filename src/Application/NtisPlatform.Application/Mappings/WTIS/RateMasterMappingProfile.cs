using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Mappings.WTIS;

/// <summary>
/// AutoMapper profile for Rate Master
/// </summary>
public class RateMasterMappingProfile : Profile
{
    public RateMasterMappingProfile()
    {
        // Entity to DTO (simple mapping)
        CreateMap<RateMasterEntity, RateMasterDto>();

        // WithJoins to DTO (for display with master names)
        CreateMap<RateMasterWithJoins, RateMasterDto>();

        // Create DTO to Entity
        CreateMap<CreateRateMasterDto, RateMasterEntity>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Update DTO to Entity
        CreateMap<UpdateRateMasterDto, RateMasterEntity>()
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
