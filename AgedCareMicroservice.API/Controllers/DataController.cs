using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tikka.ServicesAustralia.Services;

namespace MiniMicroservice.API.Controllers;

//[Authorize]
[Route("api/[controller]"), ApiController]
public class DataController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public DataController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet("GetExampleData1")]
    public async Task<ActionResult<string>> GetExampleData1(bool forceNewToken)
    {
        var (log,accessToken) = _authenticationService.GetAccessToken(forceNewToken);
        return await Task.FromResult(Ok(log));
    }

    [HttpGet("GetExampleData2")]
    public async Task<ActionResult<string>> GetExampleData2(bool forceNewToken)
    {
        var (log, accessToken) = _authenticationService.GetAccessToken(forceNewToken);
        return await Task.FromResult(Ok(log));
    }

    [HttpGet("GetExampleData3")]
    public async Task<ActionResult<string>> GetExampleData3(bool forceNewToken)
    {
        var (log, accessToken) = _authenticationService.GetAccessToken(forceNewToken);
        return await Task.FromResult(Ok(log));
    }
}