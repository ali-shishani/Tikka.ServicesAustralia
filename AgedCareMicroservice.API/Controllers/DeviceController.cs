using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tikka.ServicesAustralia.Services;

namespace MiniMicroservice.API.Controllers;

//[Authorize]
[Route("api/[controller]"), ApiController]
public class DeviceController : ControllerBase
{
    //private readonly List<string> _list = ["Bahram Bayramzade", "Nadir Zamanov", "Gulya Abbasova", "Kenan Aliyev"];
    private readonly ISADeviceService _deviceService;

    public DeviceController(ISADeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet("GetInfo")]
    public async Task<ActionResult<List<string>>> GetInfo()
    {
        var result = _deviceService.GetDeviceInfo();
        return await Task.FromResult(Ok(result));
    }

    [HttpPut("Activate")]
    public async Task<ActionResult<string>>Activate(string activationCode)
    {
        var resultLog = _deviceService.Activate(activationCode);
        return await Task.FromResult(Ok(resultLog));
    }
}