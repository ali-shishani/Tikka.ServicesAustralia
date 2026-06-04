namespace Horus.API.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<User?> GetUserByConfirmationCodeAsync(string code);
    Task<bool> SaveChangesAsync();
}