﻿namespace TrackMS.Domain.Entities;

public class Permission
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Permission(string id, string name, string description = "")
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
