using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Mappings.WTIS;

/// <summary>
/// AutoMapper profile for Pipe Size
/// </summary>
public class PipeSizeMappingProfile : Profile
{
    public PipeSizeMappingProfile()
    {
        // Entity to DTO
        CreateMap<PipeSizeMasterEntity, PipeSizeDto>();

        // Create DTO to Entity
        CreateMap<CreatePipeSizeDto, PipeSizeMasterEntity>();

        // Update DTO to Entity
        CreateMap<UpdatePipeSizeDto, PipeSizeMasterEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
