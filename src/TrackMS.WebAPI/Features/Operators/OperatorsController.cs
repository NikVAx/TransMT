using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Operators.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.Operators;

[Route("api/[controller]")]
[ApiController]
public class OperatorsController : ControllerBase
{
    private readonly OperatorsService _operatorsService;

    public OperatorsController(OperatorsService operatorsService)
    {
        _operatorsService = operatorsService;
    }

    [HttpGet]
    [Authorize(policy: PermissionKeys.CanReadOperator)]
    public async Task<ActionResult<PageResponseDto<GetVehicleOperatorDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _operatorsService.GetOperatorsPageAsync(getPageDto.PageSize, getPageDto.PageIndex);
    }

    [HttpGet("{id}")]
    [Authorize(policy: PermissionKeys.CanReadOperator)]
    public async Task<ActionResult<GetVehicleOperatorDto>> Get(string id)
    {
        return await _operatorsService.GetOperatorByIdAsync(id);
    }

    [HttpPost]
    [Authorize(policy: PermissionKeys.CanCreateOperator)]
    public async Task<ActionResult<GetVehicleOperatorDto>> Post([FromBody] CreateVehicleOperatorDto createDto)
    {
        var vehicle = await _operatorsService.CreateOperatorAsync(createDto);

        return vehicle;
    }

    [HttpPatch("{id}")]
    [Authorize(policy: PermissionKeys.CanUpdateOperator)]
    public async Task<ActionResult<GetVehicleOperatorDto>> Patch(string id, [FromBody] PatchVehicleOperatorDto patchDto)
    {
        return await _operatorsService.EditOperatorByIdAsync(id, patchDto);
    }

    [HttpDelete("{id}")]
    [Authorize(policy: PermissionKeys.CanDeleteOperator)]
    public async Task<ActionResult> Delete(string id)
    {
        await _operatorsService.DeleteOperatorByIdAsync(id);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(policy: PermissionKeys.CanDeleteOperator)]
    public async Task<ActionResult> Delete(DeleteManyDto<string> deleteManyDto)
    {
        await _operatorsService.DeleteManyOperatorsAsync(deleteManyDto);

        return NoContent();
    }
}
