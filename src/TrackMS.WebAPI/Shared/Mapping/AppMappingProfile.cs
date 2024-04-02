using AutoMapper;
using TrackMS.AuthService.Entities;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Features.Roles.DTO;
using TrackMS.WebAPI.Features.Users.DTO;

namespace TrackMS.WebAPI.Shared.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Building, GetBuildingDto>();
        CreateMap<Vehicle, GetVehicleDto>();
        CreateMap<User, GetUserDto>();
        CreateMap<Role, GetRoleDto>();
        CreateMap<User, GetUserWithRolesDto>();
    }
}
