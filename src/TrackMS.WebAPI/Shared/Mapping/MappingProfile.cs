﻿using AutoMapper;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Features.Devices.DTO;
using TrackMS.WebAPI.Features.GeoZones.DTO;
using TrackMS.WebAPI.Features.IdentityManagement.DTO;
using TrackMS.WebAPI.Features.IdentityManagement.Permissions.DTO;
using TrackMS.WebAPI.Features.IdentityManagement.Roles.DTO;
using TrackMS.WebAPI.Features.Users.DTO;
using NetTopologySuite.Geometries;
using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.Shared.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Identity Management Mapping
        CreateMap<Permission, GetPermissionDto>();

        CreateMap<Role, GetRoleDto>();
        CreateMap<Role, GetRoleWithPermissionsDto>();
        CreateMap<Role, GetRoleWithShortPermissionsDto>()
            .ForMember(
                dest => dest.Permissions, 
                src => src.MapFrom(s => s.Permissions.Select(x => x.Name))
            );

        CreateMap<User, GetUserDto>();
        CreateMap<User, GetUserWithRolesDto>();
        #endregion

        CreateMap<Building, GetBuildingDto>()
            .ForMember(
                dest => dest.Location, 
                src => src.MapFrom(s => s.Location)
            );

        CreateMap<Vehicle, GetVehicleDto>();
        CreateMap<VehicleOperator, GetVehicleOperatorDto>();
        CreateMap<Device, GetDeviceDto>();
        CreateMap<GeoZone, GetGeoZoneDto>();

        CreateMap<GeoPoint, Point>()
            .ConstructUsing(x => new Point(x.Lng, x.Lat));
        CreateMap<Point, GeoPoint>()
            .ConstructUsing(x => new GeoPoint(x.Y, x.X));
        CreateMap<Polygon, IEnumerable<GeoPoint>>()
            .ConstructUsing(x => x.Coordinates
                .Select(c => new GeoPoint(c.Y, c.X))
                .Take(x.Coordinates.Length - 1)
                .ToList());
    }
}
