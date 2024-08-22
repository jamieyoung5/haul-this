using HaulThis.Models;
using HaulThis.Services;
using Microsoft.Extensions.Configuration;

namespace HaulThis.Test.Integration;

public class ManageEmployeesTest : IClassFixture<TestDatabaseFixture>
{
    private readonly IUserService _userService;

    public ManageEmployeesTest(TestDatabaseFixture fixture)
    {
        var databaseService = new DatabaseService("");
        _userService = new UserService(databaseService);
    }

    [Fact]
    public async Task AddUser_ShouldAddUserToDatabase()
    {
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            Role = Role.Driver,
            CreatedAt = DateTime.UtcNow
        };
        
        int result = await _userService.AddUserAsync(user);
        var users = await _userService.GetAllUsersAsync();
        
        Assert.Equal(1, result);
        Assert.Contains(users, u => u.Email == "john.doe@example.com");
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveUserFromDatabase()
    {
        var user = new User
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            PhoneNumber = "0987654321",
            Address = "456 Another St",
            Role = Role.Administrator,
            CreatedAt = DateTime.UtcNow
        };

        int userId = await _userService.AddUserAsync(user);
        var usersBeforeDeletion = await _userService.GetAllUsersAsync();
        Assert.Contains(usersBeforeDeletion, u => u.Email == "jane.doe@example.com");
        
        int result = await _userService.DeleteUserAsync(userId);
        var usersAfterDeletion = await _userService.GetAllUsersAsync();
        
        Assert.Equal(1, result);
        Assert.DoesNotContain(usersAfterDeletion, u => u.Email == "jane.doe@example.com");
    }
    
    [Fact]
    public async Task UpdateUser_ShouldModifyUserInDatabase()
    {
        var user = new User
        {
            FirstName = "Mark",
            LastName = "Smith",
            Email = "mark.smith@example.com",
            PhoneNumber = "5551234567",
            Address = "789 New St",
            Role = Role.Driver,
            CreatedAt = DateTime.UtcNow
        };

        int userId = await _userService.AddUserAsync(user);
        var userToUpdate = await _userService.GetUserByIdAsync(userId);
        userToUpdate.LastName = "UpdatedSmith";
        
        int result = await _userService.UpdateUserAsync(userToUpdate);
        var updatedUser = await _userService.GetUserByIdAsync(userId);
        
        Assert.Equal(1, result);
        Assert.Equal("UpdatedSmith", updatedUser.LastName);
    }
}

public class TestDatabaseFixture : IDisposable
{
    public IConfiguration Configuration { get; }

    public TestDatabaseFixture()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public void Dispose()
    {
        // Clean up database or other resources if needed
    }
}