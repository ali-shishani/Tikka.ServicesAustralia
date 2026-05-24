using System.Security.Claims;
using Tikka.ServicesAustralia.Core.Models;
using Tikka.ServicesAustralia.Models.Responses;

namespace Tikka.ServicesAustralia.Services;

public interface ISADeviceService
{
    Task<(DeviceInformationResponse response, List<AppException> errors)> GetDeviceInfo();
    Task<(string response, List<AppException> errors)> Activate(string activationCode);
    Task<(string response, List<AppException> errors)> RefreshKey();
}