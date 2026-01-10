using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Mappings.WTIS;

/// <summary>
/// AutoMapper profile for Connection Type
/// </summary>
public class ConnectionTypeMappingProfile : Profile
{
    public ConnectionTypeMappingProfile()
    {
        // Entity to DTO
        CreateMap<ConnectionTypeMasterEntity, ConnectionTypeDto>();

        // Create DTO to Entity
        CreateMap<CreateConnectionTypeDto, ConnectionTypeMasterEntity>();

        // Update DTO to Entity
        CreateMap<UpdateConnectionTypeDto, ConnectionTypeMasterEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
