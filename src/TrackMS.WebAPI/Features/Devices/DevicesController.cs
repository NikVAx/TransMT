using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.Devices.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.Devices;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly DevicesService _devicesService;

    public DevicesController(DevicesService devicesService)
    {
        _devicesService = devicesService;
    }

    [HttpGet]
    [Authorize(policy: PermissionKeys.CanReadDevice)]
    public async Task<ActionResult<PageResponseDto<GetDeviceDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _devicesService.GetDevicesPageAsync(getPageDto.PageSize, getPageDto.PageIndex);
    }

    [HttpGet("{id}")]
    [Authorize(policy: PermissionKeys.CanReadDevice)]
    public async Task<ActionResult<GetDeviceDto>> Get(string id)
    {
        return await _devicesService.GetDeviceByIdAsync(id);
    }

    [HttpPost]
    [Authorize(policy: PermissionKeys.CanCreateDevice)]
    public async Task<ActionResult<GetDeviceDto>> Post([FromBody] CreateDeviceDto createDto)
    {
        var vehicle = await _devicesService.CreateDeviceAsync(createDto);

        return vehicle;
    }

    [HttpPatch("{id}")]
    [Authorize(policy: PermissionKeys.CanUpdateDevice)]
    public async Task<ActionResult<GetDeviceDto>> Patch(string id, [FromBody] PatchDeviceDto patchDto)
    {
        return await _devicesService.EditDeviceByIdAsync(id, patchDto);
    }

    [HttpDelete("{id}")]
    [Authorize(policy: PermissionKeys.CanDeleteDevice)]
    public async Task<ActionResult> Delete(string id)
    {
        await _devicesService.DeleteDeviceByIdAsync(id);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(policy: PermissionKeys.CanDeleteDevice)]
    public async Task<ActionResult> Delete(DeleteManyDto<string> deleteManyDto)
    {
        await _devicesService.DeleteManyDevicesAsync(deleteManyDto);

        return NoContent();
    }
}
