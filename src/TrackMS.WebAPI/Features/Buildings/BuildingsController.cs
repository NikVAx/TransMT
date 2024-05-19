using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.Buildings;

[Route("api/[controller]")]
[ApiController]
public class BuildingsController : ControllerBase
{
    private readonly BuildingsService _buildingsService;

    public BuildingsController(BuildingsService buildingsService)
    {
        _buildingsService = buildingsService;
    }

    [HttpGet]
    [Authorize(policy: PermissionKeys.CanReadBuilding)]
    public async Task<ActionResult<PageResponseDto<GetBuildingDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _buildingsService.GetBuildingsPageAsync(getPageDto.PageSize, getPageDto.PageIndex);
    }

    [HttpGet("{id}", Name = "GetBuilding")]
    [Authorize(policy: PermissionKeys.CanReadBuilding)]
    public async Task<ActionResult<GetBuildingDto>> Get(string id)
    {
        return Ok(await _buildingsService.GetBuildingByIdAsync(id));
    }

    [HttpPost]
    [Authorize(policy: PermissionKeys.CanCreateBuilding)]
    public async Task<ActionResult<GetBuildingDto>> Post([FromBody] CreateBuildingDto createBuildingDto)
    {
        var building = await _buildingsService.CreateBuildingAsync(createBuildingDto);

        return CreatedAtRoute("GetBuilding", new { building.Id }, building);
    }

    [HttpPatch("{id}")]
    [Authorize(policy: PermissionKeys.CanUpdateBuilding)]
    public async Task<ActionResult<GetBuildingDto>> Patch(string id, [FromBody] PatchBuildingDto patchBuildingDto)
    {
        return Ok(await _buildingsService.EditBuildingByIdAsync(id, patchBuildingDto));
    }

    [HttpDelete("{id}")]
    [Authorize(policy: PermissionKeys.CanDeleteBuilding)]
    public async Task<ActionResult> Delete(string id)
    {
        await _buildingsService.DeleteBuildingByIdAsync(id);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(policy: PermissionKeys.CanDeleteBuilding)]
    public async Task<ActionResult> Delete(DeleteManyDto<string> deleteManyDto)
    {
        await _buildingsService.DeleteManyBuidlingsAsync(deleteManyDto);

        return NoContent();
    }
}
