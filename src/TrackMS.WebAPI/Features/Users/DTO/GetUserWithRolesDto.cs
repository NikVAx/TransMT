﻿using TrackMS.WebAPI.Features.IdentityManagement.DTO;

namespace TrackMS.WebAPI.Features.Users.DTO;

public class GetUserWithRolesDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public IEnumerable<GetRoleWithPermissionsDto> Roles { get; set; }
}
