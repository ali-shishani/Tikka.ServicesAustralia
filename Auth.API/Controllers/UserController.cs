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
    }
}
