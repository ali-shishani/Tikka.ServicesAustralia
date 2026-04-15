using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tikka.ServicesAustralia.Services;

namespace MiniMicroservice.API.Controllers;

//[Authorize]
[Route("api/[controller]"), ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet("GetAccessToken")]
    public async Task<ActionResult<string>> GetAccessToken(bool forceNewToken)
    {
        var (log,accessToken) = _authenticationService.GetAccessToken(forceNewToken);
        return await Task.FromResult(Ok(log));
    }
}