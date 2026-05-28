using Tikka.ServicesAustralia.Core.Models;

namespace Horus.API.Exceptions;

public class LoginException(string message, string? details = null)
    : AppException(message, details);

public class RegisterException(string message, string? details = null)
    : AppException(message, details);