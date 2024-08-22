using HaulThis.Models;

namespace HaulThis.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int userId);
    Task<int> AddUserAsync(User user);
    Task<int> UpdateUserAsync(User user);
    Task<int> DeleteUserAsync(int userId);
}