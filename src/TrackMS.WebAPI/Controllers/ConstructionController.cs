using Microsoft.AspNetCore.Mvc;
using TrackMS.Domain.Abstractions;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;

namespace TrackMS.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConstructionController : ControllerBase
{
    private readonly ICrudService<Construction, string> _constructionService;

    public ConstructionController(ICrudService<Construction, string> constructionService)
    {
        _constructionService = constructionService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetConstructionDto>> Get(string id)
    {
        var result = await _constructionService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        var construction = result.Object;

        return Ok(
            new GetConstructionDto
            {
                Id = construction.Id,
                Address = construction.Address,
                Location = construction.Location
            });
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateConstructionDto constructionDto)
    {
        var construction = new Construction
        {
            Address = constructionDto.Address,
            Location = constructionDto.Location
        };

        var createResult = await _constructionService.CreateAsync(construction);

        if(!createResult.Succeeded)
        {
            return BadRequest(createResult);
        }

        return Created("api/constructions/{id}", construction);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] PatchConstructionDto constructionDto)
    {
        var result = await _constructionService.GetByIdAsync(id);

        if (!result.Succeeded)
        {
            return NotFound(result);
        }

        var construction = result.Object;

        construction.Address = constructionDto.Address is null ? construction.Address : constructionDto.Address;
        construction.Location = constructionDto.Location is null ? construction.Location : constructionDto.Location;

        var updateResult = await _constructionService.UpdateAsync(construction);

        if (!updateResult.Succeeded)
        {
            return BadRequest(updateResult);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _constructionService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        return NoContent();
    }
}
