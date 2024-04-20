using System.Reflection;
using TrackMS.Domain.Entities;

namespace TrackMS.WebAPI.Shared.Models;

public static class PermissionsKeys
{
    public const string CanReadUser          = "USER_CAN_READ";
    public const string CanCreateUser        = "USER_CAN_CREATE";
    public const string CanUpdateUser        = "USER_CAN_UPDATE";
    public const string CanDeleteUser        = "USER_CAN_DELETE";
                                             
    public const string CanReadRole          = "ROLE_CAN_READ";
    public const string CanCreateRole        = "ROLE_CAN_CREATE";
    public const string CanUpdateRole        = "ROLE_CAN_UPDATE";
    public const string CanDeleteRole        = "ROLE_CAN_DELETE";
                                             
    public const string CanReadVehicle       = "VEHICLE_CAN_READ";
    public const string CanCreateVehicle     = "VEHICLE_CAN_CREATE";
    public const string CanUpdateVehicle     = "VEHICLE_CAN_UPDATE";
    public const string CanDeleteVehicle     = "VEHICLE_CAN_DELETE";
                                             
    public const string CanReadDevice        = "DEVICE_CAN_READ";
    public const string CanCreateDevice      = "DEVICE_CAN_CREATE";
    public const string CanUpdateDevice      = "DEVICE_CAN_UPDATE";
    public const string CanDeleteDevice      = "DEVICE_CAN_DELETE";
                                             
    public const string CanReadGeoZone       = "GEOZONE_CAN_READ";
    public const string CanCreateGeoZone     = "GEOZONE_CAN_CREATE";
    public const string CanUpdateGeoZone     = "GEOZONE_CAN_UPDATE";
    public const string CanDeleteGeoZone     = "GEOZONE_CAN_DELETE";
}

public class Permissions
{
    private static IEnumerable<Permission> CreateDefaultPermissionsWithReflection()
    {
        var permissions = typeof(PermissionsKeys)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(x => 
            {
                string id = (x.GetRawConstantValue() as string)!;
                return new Permission(id: id, name: id);
            }).ToList();

        return permissions;
    }

    public static IEnumerable<Permission> GetPermissions()
    {
        return CreateDefaultPermissionsWithReflection();
    }
}
