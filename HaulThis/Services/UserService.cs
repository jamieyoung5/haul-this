using HaulThis.Models;

namespace HaulThis.Services;

public class UserService(IDatabaseService databaseService) : IUserService
{
    private const string GetAllUsersQuery =  "SELECT u.Id, u.firstName, u.lastName, u.email, u.phoneNumber, u.address, u.createdAt, u.updatedAt, r.roleName FROM users u JOIN role r ON u.roleId = r.Id";
    private const string GetAllUsersByIdQuery = "SELECT u.Id, u.firstName, u.lastName, u.email, u.phoneNumber, u.address, u.createdAt, u.updatedAt, r.roleName FROM users u JOIN role r ON u.roleId = r.Id WHERE u.Id = @p0";
    
    private const string AddUserStmt = """
                                       INSERT INTO users (firstName, lastName, email, phoneNumber, address, roleId, createdAt)
                                                             VALUES (@p0, @p1, @p2, @p3, @p4, (SELECT Id FROM role WHERE roleName = @p5), @p6)
                                       """;
    private const string UpdateUserStmt = """
                                          UPDATE users 
                                                                SET firstName = @p0, lastName = @p1, email = @p2, phoneNumber = @p3, 
                                                                    address = @p4, roleId = (SELECT Id FROM role WHERE roleName = @p5), updatedAt = @p6
                                                                WHERE Id = @p7
                                          """;
    private const string DeleteUserStmt = "DELETE FROM users WHERE id = @p0";
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = new List<User>();

        using (var reader = databaseService.Query(GetAllUsersQuery))
        {
            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    PhoneNumber = reader.GetString(4),
                    Address = reader.GetString(5),
                    CreatedAt = reader.GetDateTime(6),
                    UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                    Role = Enum.Parse<Role>(reader.GetString(8))
                });
            }
        }

        return await Task.FromResult(users);
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var reader = databaseService.QueryRow(GetAllUsersByIdQuery, userId);

        if (reader.IsDBNull(0))
        {
            throw new InvalidOperationException("User not found");
        }
        
        return await Task.FromResult(new User
        {
            Id = reader.GetInt32(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            Email = reader.GetString(3),
            PhoneNumber = reader.GetString(4),
            Address = reader.GetString(5),
            CreatedAt = reader.GetDateTime(6),
            UpdatedAt = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
            Role = Enum.Parse<Role>(reader.GetString(8))
        });
    }

    public async Task<int> AddUserAsync(User user)
    {
        return await Task.FromResult(databaseService.Execute(AddUserStmt, user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.Address, user.Role.ToString(), user.CreatedAt));
    }

    public async Task<int> UpdateUserAsync(User user)
    {
        return await Task.FromResult(databaseService.Execute(UpdateUserStmt, user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.Address, user.Role.ToString(), DateTime.UtcNow, user.Id));
    }

    public async Task<int> DeleteUserAsync(int userId)
    {
        return await Task.FromResult(databaseService.Execute(DeleteUserStmt, userId));
    }
}