using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Tikka.ServicesAustralia.Core.Models;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Services;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public async Task<ActionResult<ApiResponse<DeviceInformationResponse>>> GetInfo()
    {
        var (result, errors) = await _deviceService.GetDeviceInfo();
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<DeviceInformationResponse>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<DeviceInformationResponse>.SuccessResponse(result)));
    }

    [HttpPut("Activate")]
    public async Task<ActionResult<ApiResponse<string>>> Activate(string activationCode)
    {
        var (result, errors) = await _deviceService.Activate(activationCode);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<string>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<string>.SuccessResponse(result)));
    }

    [HttpPut("RefreshKey")]
    public async Task<ActionResult<ApiResponse<string>>> RefreshKey()
    {
        var (result, errors) = await _deviceService.RefreshKey();
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<string>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<string>.SuccessResponse(result)));
    }
}