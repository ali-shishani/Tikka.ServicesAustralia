using Auth.API.Models.Responses;
using Tikka.ServicesAustralia.Core.Models;

namespace Horus.API.Services;

public interface IUserService
{
    Task<(List<GetUserResponse> response, List<AppException> errors)> GetAll();
    Task<(GetUserResponse response, List<AppException> errors)> RegisterUserAsync(RegisterRequest request);
    Task<(GetUserResponse response, List<AppException> errors)> UpdateUserAsync(UpdateUserRequest request);
    Task<(bool response, List<AppException> errors)> DeleteUserAsync(Guid userId);
}