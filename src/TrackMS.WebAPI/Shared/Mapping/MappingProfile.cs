using AutoMapper;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Features.GeoZones.DTO;
using TrackMS.WebAPI.Features.IdentityManagement.DTO;
using TrackMS.WebAPI.Features.Users.DTO;

namespace TrackMS.WebAPI.Shared.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Identity Management Mapping
        CreateMap<Permission, GetPermissionDto>();

        CreateMap<Role, GetRoleDto>();
        CreateMap<Role, GetRoleWithPermissionsDto>();

        CreateMap<User, GetUserDto>();
        CreateMap<User, GetUserWithRolesDto>();
        #endregion

        CreateMap<Building, GetBuildingDto>();
        CreateMap<Vehicle, GetVehicleDto>();
        CreateMap<VehicleOperator, GetVehicleOperatorDto>();
        CreateMap<GeoZone, GetGeoZoneDto>();
    }
}
