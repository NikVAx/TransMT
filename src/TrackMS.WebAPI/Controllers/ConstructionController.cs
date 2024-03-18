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
        try
        {
            var construction = await _constructionService.GetByIdAsync(id);

            return Ok(
                new GetConstructionDto
                {
                    Id = construction.Id,
                    Address = construction.Address,
                    Location = construction.Location
                });

        }
        catch(ApplicationException ex)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateConstructionDto constructionDto)
    {
        var construction = new Construction
        {
            Address = constructionDto.Address,
            Location = constructionDto.Location
        };

        await _constructionService.CreateAsync(construction);

        return Created("api/constructions/{id}", construction);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] PatchConstructionDto constructionDto)
    {
        try
        {
            var construction = await _constructionService.GetByIdAsync(id);

            construction.Address = constructionDto.Address is null ? construction.Address : constructionDto.Address;
            construction.Location = constructionDto.Location is null ? construction.Location : constructionDto.Location;

            await _constructionService.UpdateAsync(construction);
        }
        catch(ApplicationException ex)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        try
        {
            var construction = await _constructionService.GetByIdAsync(id);
        }
        catch(ApplicationException ex)
        {
            return NotFound();
        }

        return NoContent();
    }
}
