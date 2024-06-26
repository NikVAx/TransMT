﻿using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Buildings.DTO;

namespace TrackMS.WebAPI.DTO;

public class GetVehicleDto
{
    public string Id { get; set; }
    public string Number { get; set; }
    public string Type { get; set; }
    public string OperatingStatus { get; set; }
    public GetBuildingDto? StorageArea { get; set; }
}
