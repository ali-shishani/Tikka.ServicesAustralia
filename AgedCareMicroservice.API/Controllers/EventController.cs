using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mono.TextTemplating;
using Tikka.ServicesAustralia.Models.Requests;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Services;

namespace MiniMicroservice.API.Controllers;

//[Authorize]
[Route("api/[controller]"), ApiController]
public class EventController : ControllerBase
{
    private readonly IDataService _dataService;

    public EventController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("QueryEntryEvents")]
    public async Task<ActionResult<List<QueryEntryEventsResponse>>> QueryEntryEvents(
        string? careRecipientId,
        string? externalReferenceId,
        string? entryDateFrom,
        string? entryDateTo,
        int limit,
        string? page,
        string? sort)
    {

        var result = await _dataService.QueryEntryEvents(careRecipientId, externalReferenceId, entryDateFrom, entryDateTo, limit, page, sort);
        return await Task.FromResult(Ok(result));
    }

    [HttpPost("CreateEntryEvent")]
    public async Task<ActionResult<CreateEntryEventResponse>> CreateEntryEvent(string tempAccessKey, CreateEntryEventRequest request)
    {
        var result = await _dataService.CreateEntryEvent(tempAccessKey, request);
        return await Task.FromResult(Ok(result));
    }

    [HttpGet("GetEntryEventDetails/{eventId}")]
    public async Task<ActionResult<GetEntryEventDetailsResponse>> GetEntryEventDetails(string? eventId)
    {
        var result = await _dataService.GetEntryEventDetails(eventId);
        return await Task.FromResult(Ok(result));
    }

    [HttpPut("UpdateEntryEvent/{eventId}")]
    public async Task<ActionResult<UpdateEntryEventResponse>> UpdateEntryEvent(string? eventId, UpdateEntryEventRequest request)
    {
        var result = await _dataService.UpdateEntryEvent(eventId, request);
        return await Task.FromResult(Ok(result));
    }

    [HttpDelete("DeleteEntryEvent/{eventId}")]
    public async Task<ActionResult<DeleteEntryEventResponse>> DeleteEntryEvent(string? eventId)
    {
        var result = await _dataService.DeleteEntryEvent(eventId);
        return await Task.FromResult(Ok(result));
    }
}