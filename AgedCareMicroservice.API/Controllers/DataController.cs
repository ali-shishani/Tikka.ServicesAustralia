using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Services;

namespace MiniMicroservice.API.Controllers;

//[Authorize]
[Route("api/[controller]"), ApiController]
public class DataController : ControllerBase
{
    private readonly IDataService _dataService;

    public DataController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("CareRecipientSearch")]
    public async Task<ActionResult<CareRecipientSearchResponse>> CareRecipientSearch(
        string? careRecipientId, 
        string? firstName, 
        string? middleName, 
        string? lastName, 
        string? gender, 
        string? birthDate, 
        string? postCode, 
        string? State)
    {
        var result = _dataService.CareRecipientSearch(careRecipientId, firstName, middleName, lastName, gender, birthDate, postCode, State);
        return await Task.FromResult(Ok(result));
    }

    [HttpGet("ResidentialCareEntryEvent")]
    public async Task<ActionResult<ResidentialCareEntryEventResponse>> ResidentialCareEntryEvent()
    {
        var result = _dataService.ResidentialCareEntry();
        return await Task.FromResult(Ok(result));
    }
}