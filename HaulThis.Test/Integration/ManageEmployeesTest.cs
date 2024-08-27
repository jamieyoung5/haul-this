using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Repository;

namespace HaulThis.Test.Integration;

[Collection("Sequential Tests")]
public class ManageEmployeesTest : DisposableIntegrationTest
{
    private readonly IUserRepository _userRepository;

    public ManageEmployeesTest()
    {
        _userRepository = new UserRepository(_databaseService);
    }

    [Fact]
    public async Task AddUser_ShouldAddUserToDatabase_WhenValidUserIsProvided()
    {
        // Arrange
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

        // Act
        int result = await _userRepository.AddUserAsync(user);

        // Assert
        IEnumerable<User> users = await _userRepository.GetAllUsersAsync();
        Assert.Equal(1, result);
        Assert.Contains(users, u => u.Email == "john.doe@example.com");
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveUserFromDatabase_WhenUserIdIsExists()
    {
        // Arrange
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

        int userId = await _userRepository.AddUserAsync(user);
        IEnumerable<User> usersBeforeDeletion = await _userRepository.GetAllEmployeesAsync();
        Assert.Contains(usersBeforeDeletion, u => u.Email == "jane.doe@example.com");

        // Act
        int result = await _userRepository.DeleteUserAsync(userId);

        // Assert
        IEnumerable<User> usersAfterDeletion = await _userRepository.GetAllEmployeesAsync();
        Assert.Equal(1, result);
        Assert.DoesNotContain(usersAfterDeletion, u => u.Email == "jane.doe@example.com");
    }

    [Fact]
    public async Task UpdateUser_ShouldModifyUserInDatabase_WhenEditsAreValid()
    {
        // Arrange
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

        int userId = await _userRepository.AddUserAsync(user);
        var userToUpdate = await _userRepository.GetUserByIdAsync(userId);
        userToUpdate.LastName = "UpdatedSmith";

        // Act
        int result = await _userRepository.UpdateUserAsync(userToUpdate);

        // Assert
        var updatedUser = await _userRepository.GetUserByIdAsync(userId);
        Assert.Equal(1, result);
        Assert.Equal("UpdatedSmith", updatedUser.LastName);
    }
}