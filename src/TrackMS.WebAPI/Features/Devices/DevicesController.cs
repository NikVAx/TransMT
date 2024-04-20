﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.Devices.DTO;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.Devices
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DevicesService _operatorsService;

    public DevicesController(DevicesService operatorsService)
    {
        _operatorsService = operatorsService;
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetDeviceDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _operatorsService.GetDevicesPageAsync(getPageDto.PageSize, getPageDto.PageIndex);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetDeviceDto>> Get(string id)
    {
        return await _operatorsService.GetDeviceByIdAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<GetDeviceDto>> Post([FromBody] CreateDeviceDto createDto)
    {
        var vehicle = await _operatorsService.CreateDeviceAsync(createDto);

        return vehicle;
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<GetDeviceDto>> Patch(string id, [FromBody] PatchDeviceDto patchDto)
    {
        return await _operatorsService.EditDeviceByIdAsync(id, patchDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _operatorsService.DeleteDeviceByIdAsync(id);

        return NoContent();
    }
    }
}
