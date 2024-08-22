using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Test.ViewModels;

public class UserListViewModelTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserListViewModel _viewModel;

    public UserListViewModelTests()
    {
        _mockUserService = new Mock<IUserService>();
        _viewModel = new UserListViewModel(_mockUserService.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeUsersCollection()
    {
        Assert.NotNull(_viewModel.Users);
        Assert.IsType<ObservableCollection<User>>(_viewModel.Users);
    }

    [Fact]
    public void Constructor_ShouldCallLoadUsers()
    {
        _mockUserService.Verify(s => s.GetAllUsersAsync(), Times.Once);
    }

    [Fact]
    public void LoadUsers_ShouldUpdateUsersCollection()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, FirstName = "John", LastName = "Doe" },
            new User { Id = 2, FirstName = "Jane", LastName = "Doe" }
        };
        _mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var viewModel = new UserListViewModel(_mockUserService.Object);

        // Assert
        Assert.Equal(users.Count, viewModel.Users.Count);
        for (int i = 0; i < users.Count; i++)
        {
            Assert.Equal(users[i].Id, viewModel.Users[i].Id);
            Assert.Equal(users[i].FirstName, viewModel.Users[i].FirstName);
            Assert.Equal(users[i].LastName, viewModel.Users[i].LastName);
        }
    }
    
}