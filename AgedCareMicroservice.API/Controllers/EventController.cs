using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mono.TextTemplating;
using Tikka.ServicesAustralia.Core.Models;
using Tikka.ServicesAustralia.Models.Requests;
using Tikka.ServicesAustralia.Models.Responses;
using Tikka.ServicesAustralia.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace MiniMicroservice.API.Controllers;

[Authorize]
[Route("api/[controller]"), ApiController]
public class EventController : ControllerBase
{
    private readonly IDataService _dataService;

    public EventController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("QueryResidentialEntryEvents")]
    public async Task<ActionResult<ApiResponse<List<QueryEntryEventsResponse>>>> QueryResidentialEntryEvents(
        string? careRecipientId,
        string? externalReferenceId,
        string? entryDateFrom,
        string? entryDateTo,
        int limit,
        string? page,
        string? sort)
    {
        var (result, errors) = await _dataService.QueryResidentialEntryEvents(careRecipientId, externalReferenceId, entryDateFrom, entryDateTo, limit, page, sort);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<List<QueryEntryEventsResponse>>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<List<QueryEntryEventsResponse>>.SuccessResponse(result)));
    }

    [HttpGet("QueryHomeEntryEvents")]
    public async Task<ActionResult<ApiResponse<List<QueryEntryEventsResponse>>>> QueryHomeEntryEvents(
        string? careRecipientId,
        string? externalReferenceId,
        string? entryDateFrom,
        string? entryDateTo,
        int limit,
        string? page,
        string? sort)
    {
        var (result, errors) = await _dataService.QueryHomeEntryEvents(careRecipientId, externalReferenceId, entryDateFrom, entryDateTo, limit, page, sort);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<List<QueryEntryEventsResponse>>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<List<QueryEntryEventsResponse>>.SuccessResponse(result)));
    }

    [HttpPost("CreateEntryEvent")]
    public async Task<ActionResult<ApiResponse<CreateEntryEventResponse>>> CreateEntryEvent(string tempAccessKey, CreateEntryEventRequest request)
    {
        var (result, errors) = await _dataService.CreateEntryEvent(tempAccessKey, request);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<CreateEntryEventResponse>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<CreateEntryEventResponse>.SuccessResponse(result)));
    }

    [HttpGet("GetEntryEventDetails/{eventId}")]
    public async Task<ActionResult<ApiResponse<GetEntryEventDetailsResponse>>> GetEntryEventDetails(string eventId)
    {
        var (result, errors) = await _dataService.GetEntryEventDetails(eventId);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<GetEntryEventDetailsResponse>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<GetEntryEventDetailsResponse>.SuccessResponse(result)));
    }

    [HttpPut("UpdateEntryEvent/{eventId}")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateEntryEvent(string eventId, UpdateEntryEventRequest request)
    {
        var (result, errors) = await _dataService.UpdateEntryEvent(eventId, request);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<List<EntryEventHistoryResponse>>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<bool>.SuccessResponse(result)));
    }

    [HttpDelete("DeleteEntryEvent/{eventId}")]
    public async Task<ActionResult<ApiResponse<DeleteEntryEventResponse>>> DeleteEntryEvent(string eventId)
    {
        var (result, errors) = await _dataService.DeleteEntryEvent(eventId);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<List<EntryEventHistoryResponse>>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<DeleteEntryEventResponse>.SuccessResponse(result)));
    }

    [HttpGet("EntryEventHistory/{eventId}")]
    public async Task<ActionResult<ApiResponse<List<EntryEventHistoryResponse>>>> EntryEventHistory(string eventId)
    {
        var (result, errors) = await _dataService.EntryEventHistory(eventId);
        if (errors.Count > 0)
        {
            return await Task.FromResult(BadRequest(ApiResponse<List<EntryEventHistoryResponse>>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
        }

        return await Task.FromResult(Ok(ApiResponse<List<EntryEventHistoryResponse>>.SuccessResponse(result)));
    }
}