using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Enums;
using TrackMS.Domain.Interfaces;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.Buildings;

[Route("api/[controller]")]
[ApiController]
public class BuildingsController : ControllerBase
{
    private readonly ICrudService<Building, string> _buildingService;

    public BuildingsController(ICrudService<Building, string> buildingService)
    {
        _buildingService = buildingService;
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetBuildingDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        var query = _buildingService.GetEntities();

        if(getPageDto.SortBy != null && getPageDto.SortOrder != SortOrder.Descending)
        {
            query = getPageDto.SortBy switch
            {
                nameof(Building.Address) => query.OrderBy(x => x.Address),
                nameof(Building.Type) => query.OrderBy(x => x.Type),
                nameof(Building.Name) => query.OrderBy(x => x.Name),
                _ => query.OrderBy(x => x.Id),
            };
        }

        var items = await query
            .Skip(getPageDto.PageSize * getPageDto.PageIndex)
            .Take(getPageDto.PageSize)
            .Select(x => new GetBuildingDto
            {
                Id = x.Id,
                Address = x.Address,
                Location = x.Location,
                Name = x.Name,
                Type = x.Type,
            })
            .ToListAsync();

        var count = await _buildingService.GetEntities()
            .CountAsync();

        return Ok(new PageResponseDto<GetBuildingDto>
        {
            Items = items,
            PageIndex = getPageDto.PageIndex,
            PageSize = getPageDto.PageSize,
            TotalCount = count
        });
    }

    [HttpGet("{id}", Name = "GetBuilding")]
    public async Task<ActionResult<GetBuildingDto>> Get(string id)
    {
        var result = await _buildingService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        var building = result.Object;

        return Ok(
            new GetBuildingDto
            {
                Id = building.Id,
                Address = building.Address,
                Location = building.Location,
                Name = building.Name
            });
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateBuildingDto requestDto)
    {
        var building = new Building
        {
            Id = Guid.NewGuid().ToString(),
            Address = requestDto.Address,
            Location = requestDto.Location,
            Type = requestDto.Type,
            Name = requestDto.Name
        };

        var createResult = await _buildingService.CreateAsync(building);

        if(!createResult.Succeeded)
        {
            return BadRequest(createResult);
        }

        return CreatedAtRoute("GetBuilding", new { building.Id }, building);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] PatchBuildingDto requestDto)
    {
        var result = await _buildingService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        var building = result.Object;

        building.Address = requestDto.Address is null ? building.Address : requestDto.Address;
        building.Location = requestDto.Location is null ? building.Location : requestDto.Location;
        building.Type = requestDto.Type is null ? building.Type : requestDto.Type;
        building.Name = requestDto.Name is null ? building.Name : requestDto.Name;

        var updateResult = await _buildingService.UpdateAsync(building);

        if(!updateResult.Succeeded)
        {
            return BadRequest(updateResult);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _buildingService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(MultiplyDeletionRequestDto<string> requestDto)
    {
        var result = await _buildingService.DeleteManyAsync(requestDto.Keys);

        if(!result.Succeeded)
        {
            return BadRequest(result);
        }

        return NoContent();
    }
}
