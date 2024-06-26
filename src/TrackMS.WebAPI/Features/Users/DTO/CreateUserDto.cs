﻿namespace TrackMS.WebAPI.Features.Users.DTO;

public class CreateUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }

    public IEnumerable<string> Roles { get; set; }
}
