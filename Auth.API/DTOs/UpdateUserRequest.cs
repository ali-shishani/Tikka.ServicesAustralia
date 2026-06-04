namespace Horus.API.DTOs;

public class UpdateUserRequest
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required bool IsEmailConfirmed { get; set; }
}