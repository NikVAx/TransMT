using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Features.Buildings;
using TrackMS.WebAPI.Features.GeoZones.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.GeoZones;

[Route("api/[controller]")]
[ApiController]
public class GeoZonesController : ControllerBase
{
    private readonly GeoZonesService _geoZonesService;

    public GeoZonesController(GeoZonesService geoZonesService)
    {
        _geoZonesService = geoZonesService;
    }

    [HttpGet]
    [Authorize(policy: PermissionKeys.CanReadGeoZone)]
    public async Task<ActionResult<PageResponseDto<GetGeoZoneDto>>> GetGeoZones([FromQuery]PageRequestDto pageDto)
    {
        return await _geoZonesService.GetGeoZonesPageAsync(pageDto.PageSize, pageDto.PageIndex);
    }

    [HttpPost]
    [Authorize(policy: PermissionKeys.CanCreateGeoZone)]
    public async Task<ActionResult<GetGeoZoneDto>> Post(CreateGeoZoneDto createDto)
    {
        return await _geoZonesService.CreateGeoZoneAsync(createDto);
    }

    [HttpGet("{id}", Name = "GetGeoZone")]
    [Authorize(policy: PermissionKeys.CanReadGeoZone)]
    public async Task<ActionResult<GetGeoZoneDto>> Get(string id)
    {
        return Ok(await _geoZonesService.GetGeoZoneByIdAsync(id));
    }

    [HttpPatch("{id}")]
    [Authorize(policy: PermissionKeys.CanUpdateGeoZone)]
    public async Task<ActionResult<GetGeoZoneDto>> Patch(string id, PatchGeoZoneDto patchDto)
    {
        return await _geoZonesService.EditGeoZoneByIdAsync(id, patchDto);
    }

    [HttpDelete]
    [Authorize(policy: PermissionKeys.CanDeleteGeoZone)]
    public async Task<ActionResult> Delete(DeleteManyDto<string> deleteDto)
    { 
        await _geoZonesService.DeleteManyGeoZonesAsync(deleteDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(policy: PermissionKeys.CanDeleteGeoZone)]
    public async Task<ActionResult> Delete(string id)
    {
        await _geoZonesService.DeleteGeoZoneByIdAsync(id);

        return NoContent();
    }
}
