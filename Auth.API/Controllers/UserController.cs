using Auth.API.Models.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Tikka.ServicesAustralia.Core.Models;

namespace Auth.API.Controllers
{
    [Authorize]
    [Route("api/[controller]"), ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService { get; set; }

        public UserController(IUserService userService) 
        { 
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse<List<GetUserResponse>>>> GetAll()
        {
            var (result, errors) = await _userService.GetAll();
            if (errors.Count > 0)
            {
                return await Task.FromResult(BadRequest(ApiResponse<List<GetUserResponse>>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
            }

            return await Task.FromResult(Ok(ApiResponse<List<GetUserResponse>>.SuccessResponse(result)));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse<GetUserResponse>>> Register([FromBody] RegisterRequest request)
        {
            var (result, errors) = await _userService.RegisterUserAsync(request);
            if (errors.Count > 0)
            {
                return await Task.FromResult(BadRequest(ApiResponse<List<GetUserResponse>>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
            }

            return await Task.FromResult(Ok(ApiResponse<GetUserResponse>.SuccessResponse(result)));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<ApiResponse<GetUserResponse>>> Update([FromBody] UpdateUserRequest request)
        {
            var (result, errors) = await _userService.UpdateUserAsync(request);
            if (errors.Count > 0)
            {
                return await Task.FromResult(BadRequest(ApiResponse<GetUserResponse>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
            }

            return await Task.FromResult(Ok(ApiResponse<GetUserResponse>.SuccessResponse(result)));
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid userId)
        {
            var (result, errors) = await _userService.DeleteUserAsync(userId);
            if (errors.Count > 0)
            {
                return await Task.FromResult(BadRequest(ApiResponse<bool>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
            }

            return await Task.FromResult(Ok(ApiResponse<bool>.SuccessResponse(result)));
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult<ApiResponse<GetUserResponse>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(user) || !Guid.TryParse(user, out var id))
            {
                return Unauthorized("Invalid user token");
            }

            var (result, errors) = await _userService.ChangePasswordAsync(request, id);
            if (errors.Count > 0)
            {
                return await Task.FromResult(BadRequest(ApiResponse<bool>.FailureResponse(StatusCodes.Status400BadRequest, errors)));
            }

            return await Task.FromResult(Ok(ApiResponse<bool>.SuccessResponse(result)));
        }
    }
}
