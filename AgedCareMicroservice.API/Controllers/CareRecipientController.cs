using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mono.TextTemplating;
using Tikka.ServicesAustralia.Core.Models;
using Tikka.ServicesAustralia.Models.Requests;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Services;

namespace MiniMicroservice.API.Controllers;

//[Authorize]
[Route("api/[controller]"), ApiController]
public class CareRecipientController : ControllerBase
{
    private readonly IDataService _dataService;

    public CareRecipientController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("CareRecipientSearch")]
    public async Task<ActionResult<ApiResponse<CareRecipientSearchResponse>>> CareRecipientSearch(
        string? careRecipientId,
        string? firstName,
        string? middleName,
        string? lastName,
        string? gender,
        string? birthDate,
        string? postCode,
        string? State)
    {
        var (result, errors) = await _dataService.CareRecipientSearch(careRecipientId, firstName, middleName, lastName, gender, birthDate, postCode, State);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<CareRecipientSearchResponse>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<CareRecipientSearchResponse>.SuccessResponse(result)));
    }
}