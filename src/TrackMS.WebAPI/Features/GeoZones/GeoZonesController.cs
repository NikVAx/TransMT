using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.GeoZones.DTO;
using TrackMS.WebAPI.Shared.DTO;

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
    public async Task<ActionResult<PageResponseDto<GetGeoZoneDto>>> GetGeoZones([FromQuery]PageRequestDto pageDto)
    {
        return await _geoZonesService.GetGeoZonesPageAsync(pageDto.PageSize, pageDto.PageIndex);
    }

    [HttpPost]
    public async Task<ActionResult<GetGeoZoneDto>> Post(CreateGeoZoneDto createDto)
    {
        return await _geoZonesService.CreateGeoZoneAsync(createDto);
    }

    [HttpPatch("id")]
    public async Task<ActionResult<GetGeoZoneDto>> Patch(string id, PatchGeoZoneDto patchDto)
    {
        return await _geoZonesService.EditGeoZoneByIdAsync(id, patchDto);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(DeleteManyDto<string> deleteDto)
    { 
        await _geoZonesService.DeleteManyGeoZonesAsync(deleteDto);

        return NoContent();
    }
}
