using Auth.API.Models.Responses;
using Tikka.ServicesAustralia.Core.Models;

namespace Horus.API.Services;

public interface IUserService
{
    Task<(List<GetUserResponse> response, List<AppException> errors)> GetAll();
}