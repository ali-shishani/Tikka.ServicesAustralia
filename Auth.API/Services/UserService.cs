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
    private readonly RegisterRequestValidator _registerRequestValidator;
    private readonly UpdateUserRequestValidator _updateUserRequestValidator;

    public UserService(
        IUserRepository userRepository,
        IEmailService emailService,
        ILogger<UserService> logger,
        RegisterRequestValidator registerRequestValidator,
        UpdateUserRequestValidator updateUserRequestValidator)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
        _registerRequestValidator = registerRequestValidator;
        _updateUserRequestValidator = updateUserRequestValidator;
    }

    private static string GenerateRandomCode(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static string PasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
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

    public async Task<(GetUserResponse response, List<AppException> errors)> RegisterUserAsync(RegisterRequest request)
    {
        var errors = new List<AppException>();
        GetUserResponse? result = null;
        
        var validationResult = await _registerRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            errors = validationResult.Errors.Select(s => new AppException($"Validation failed", s.ErrorMessage)).ToList();
            return (result, errors);
        }

        var existingUserByEmail = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUserByEmail != null)
        {
            errors = new List<AppException>() { new AppException($"Validation failed", "Email already exists") };
            return (result, errors);
        }

        var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(request.Username);
        if (existingUserByUsername != null)
        {
            errors = new List<AppException>() { new AppException($"Validation failed", "Username already exists") };
            return (result, errors);
        }

        var user = new User
        {
            Email = request.Email.ToLower(),
            UserName = request.Username,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            IsEmailConfirmed = false,
            EmailConfirmationCode = GenerateRandomCode(),
            EmailConfirmationCodeExpireTime = DateTime.UtcNow.AddMinutes(5),
            Password = PasswordHash(request.Password)
        };

        await _userRepository.AddUserAsync(user);
        result = new GetUserResponse()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender.ToString(),
            IsEmailConfirmed = user.IsEmailConfirmed
        };

        try
        {
            await _emailService.SendConfirmationEmailAsync(user.Email, user.EmailConfirmationCode);
            return (result, errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send confirmation email to {Email}", user.Email);
            return (result, errors);
        }
    }

    public async Task<(GetUserResponse response, List<AppException> errors)> UpdateUserAsync(UpdateUserRequest request)
    {
        var errors = new List<AppException>();
        GetUserResponse? result = null;

        var validationResult = await _updateUserRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            errors = validationResult.Errors.Select(s => new AppException($"Validation failed", s.ErrorMessage)).ToList();
            return (result, errors);
        }

        var allUsers = await _userRepository.GetAllAsync();
        var existingUserByUsername = allUsers.FirstOrDefault(f => f.UserName == request.Username && f.Id != request.UserId);
        if (existingUserByUsername != null)
        {
            errors = new List<AppException>() { new AppException($"Validation failed", "Username already exists") };
            return (result, errors);
        }

        var record = await _userRepository.GetByIdAsync(request.UserId);
        if (record != null) 
        {
            record.UserName = request.Username;
            record.DateOfBirth = request.DateOfBirth;
            record.Gender = request.Gender;
            record.IsEmailConfirmed = request.IsEmailConfirmed;

            record = await _userRepository.UpdateUserAsync(record);

            result = new GetUserResponse()
            {
                Id = record.Id,
                Email = record.Email,
                UserName = record.UserName,
                DateOfBirth = record.DateOfBirth,
                Gender = record.Gender.ToString(),
                IsEmailConfirmed = record.IsEmailConfirmed
            };
        }

        return (result, errors);
    }
}