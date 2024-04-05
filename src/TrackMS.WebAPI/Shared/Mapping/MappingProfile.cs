using AutoMapper;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Features.GeoZones.DTO;
using TrackMS.WebAPI.Features.Roles.DTO;
using TrackMS.WebAPI.Features.Users.DTO;

namespace TrackMS.WebAPI.Shared.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Building, GetBuildingDto>();
        CreateMap<Vehicle, GetVehicleDto>();
        CreateMap<VehicleOperator, GetVehicleOperatorDto>();
        CreateMap<User, GetUserDto>();
        CreateMap<User, GetUserWithRolesDto>();
        CreateMap<Role, GetRoleDto>();
        CreateMap<GeoZone, GetGeoZoneDto>();
    }
}
