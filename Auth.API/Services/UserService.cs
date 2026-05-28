using Auth.API.Models.Responses;
using Newtonsoft.Json;
using Org.BouncyCastle.Tsp;
using Tikka.ServicesAustralia.Core.Configs;
using Tikka.ServicesAustralia.Core.Data.Entities;
using Tikka.ServicesAustralia.Core.Models;

namespace Horus.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IEmailService emailService,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<(List<GetUserResponse> response, List<AppException> errors)> GetAll()
    {
        var errors = new List<AppException>();

        var allUsers = await _userRepository.GetAllAsync();
        var result = allUsers.Select(s => new GetUserResponse
        {
            Id = s.Id,
            Email = s.Email,
            UserName = s.UserName,
            IsEmailConfirmed = s.IsEmailConfirmed,
            DateOfBirth = s.DateOfBirth,
            Gender = s.Gender.ToString(),
        }).ToList();

        return await Task.FromResult((result, errors));
    }
}