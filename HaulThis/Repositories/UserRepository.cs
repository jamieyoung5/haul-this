﻿using HaulThis.Models;
using HaulThis.Repository;
using HaulThis.Services;

namespace HaulThis.Repositories;

public class UserRepository(IDatabaseService databaseService) : IUserRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        const string getAllUsersQuery = """
                                        SELECT u.Id, u.firstName, u.lastName, u.email, u.phoneNumber, u.address, u.createdAt, u.updatedAt, r.roleName 
                                        FROM users u 
                                        JOIN role r ON u.roleId = r.Id;
                                        """;

        List<User> users = [];

        using (var reader = databaseService.Query(getAllUsersQuery))
        {
            while (reader.Read())
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

        return await Task.FromResult(users);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllCustomersAsync()
    {
        const string getAllCustomersQuery = """
                                            SELECT u.Id, u.firstName, u.lastName, u.email, u.phoneNumber, u.address, u.createdAt, u.updatedAt, r.roleName
                                            FROM users u 
                                            JOIN role r ON u.roleId = r.Id
                                            WHERE r.roleName = 'Customer';
                                            """;
        
        List<User> users = [];

        using (var reader = databaseService.Query(getAllCustomersQuery))
        {
            while (reader.Read())
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

        return await Task.FromResult(users);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllEmployeesAsync()
    {
        const string getAllEmployeesQuery = """
                                            SELECT u.Id, u.firstName, u.lastName, u.email, u.phoneNumber, u.address, u.createdAt, u.updatedAt, r.roleName
                                            FROM users u
                                            JOIN role r ON u.roleId = r.Id 
                                            WHERE r.roleName IN ('Administrator', 'Driver');
                                            """;
        
        List<User> users = [];

        using (var reader = databaseService.Query(getAllEmployeesQuery))
        {
            while (reader.Read())
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

        return await Task.FromResult(users);
    }

    /// <inheritdoc />
    public async Task<User> GetUserByIdAsync(int userId)
    {
        const string getAllUsersByIdQuery = """
                                            SELECT u.Id, u.firstName, u.lastName, u.email, u.phoneNumber, u.address, u.createdAt, u.updatedAt, r.roleName 
                                            FROM users u 
                                            JOIN role r ON u.roleId = r.Id WHERE u.Id = @p0;
                                            """;
        
        var reader = databaseService.QueryRow(getAllUsersByIdQuery, userId);

        if (reader.IsDBNull(0)) throw new InvalidOperationException("User not found");

        return await Task.FromResult(new User
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

    /// <inheritdoc />
    public async Task<int> AddUserAsync(User user)
    {
        const string addUserStmt = """
                                   INSERT INTO users (firstName, lastName, email, phoneNumber, address, roleId, createdAt)
                                   VALUES (@p0, @p1, @p2, @p3, @p4, (SELECT Id FROM role WHERE roleName = @p5), @p6);
                                   """;
        
        return await Task.FromResult(databaseService.Execute(addUserStmt, user.FirstName, user.LastName, user.Email,
            user.PhoneNumber, user.Address, user.Role.ToString(), user.CreatedAt));
    }

    /// <inheritdoc />
    public async Task<int> UpdateUserAsync(User user)
    {
        const string updateUserStmt = """
                                      UPDATE users 
                                      SET firstName = @p0, lastName = @p1, email = @p2, phoneNumber = @p3, address = @p4, roleId = (SELECT Id FROM role WHERE roleName = @p5), updatedAt = @p6
                                      WHERE Id = @p7;
                                      """;
        
        return await Task.FromResult(databaseService.Execute(updateUserStmt, user.FirstName, user.LastName, user.Email,
            user.PhoneNumber, user.Address, user.Role.ToString(), DateTime.UtcNow, user.Id));
    }

    /// <inheritdoc />
    public async Task<int> DeleteUserAsync(int userId)
    {
        const string deleteUserStmt = "DELETE FROM users WHERE id = @p0";
        
        return await Task.FromResult(databaseService.Execute(deleteUserStmt, userId));
    }
}