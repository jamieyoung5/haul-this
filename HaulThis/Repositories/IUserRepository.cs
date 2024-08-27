using HaulThis.Models;

namespace HaulThis.Repository;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves all users asynchronously.
    /// </summary>
    /// <returns>A collection of all users.</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();
    
    /// <summary>
    /// Retrieves all customers asynchronously.
    /// </summary>
    /// <returns>A collection of all customers.</returns>
    Task<IEnumerable<User>> GetAllCustomersAsync();
    
    /// <summary>
    /// Retrieves all employees asynchronously.
    /// </summary>
    /// <returns>A collection of all employees.</returns>
    Task<IEnumerable<User>> GetAllEmployeesAsync();
    
    /// <summary>
    /// Retrieves a specific user by their ID asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID.</returns>
    Task<User> GetUserByIdAsync(int userId);
    
    /// <summary>
    /// Adds a new user asynchronously.
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> AddUserAsync(User user);
    
    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> UpdateUserAsync(User user);
    
    /// <summary>
    /// Deletes a user by ID asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> DeleteUserAsync(int userId);
}