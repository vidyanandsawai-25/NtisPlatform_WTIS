using AutoMapper;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Application.Mappings.WTIS;

/// <summary>
/// AutoMapper profile for Consumer Account (read-only mappings)
/// </summary>
public class ConsumerAccountMappingProfile : Profile
{
    public ConsumerAccountMappingProfile()
    {
        // Basic entity to DTO (for queries without JOINs)
        CreateMap<ConsumerAccountEntity, ConsumerAccountDto>();

        // Result with master table data to DTO (for queries with JOINs - optimized)
        CreateMap<ConsumerAccountWithMasterData, ConsumerAccountDto>();
    }
}
