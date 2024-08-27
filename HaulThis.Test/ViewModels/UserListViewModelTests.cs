using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Repository;
using HaulThis.ViewModels;

namespace HaulThis.Test.ViewModels;

public class UserListViewModelTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserListViewModel _subject;

    public UserListViewModelTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _subject = new UserListViewModel(_mockUserRepository.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeUsersCollection()
    {
        Assert.NotNull(_subject.Items);
        Assert.IsType<ObservableCollection<User>>(_subject.Items);
    }

    [Fact]
    public void Constructor_ShouldCallLoadEmployee()
    {
        _mockUserRepository.Verify(s => s.GetAllEmployeesAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadCustomers_ShouldUpdateCustomersCollection()
    {
        // Arrange
        List<User> customers = new List<User>
        {
            new() { Id = 1, FirstName = "Alice", LastName = "Smith" },
            new() { Id = 2, FirstName = "Bob", LastName = "Johnson" }
        };
        _mockUserRepository.Setup(s => s.GetAllCustomersAsync()).ReturnsAsync(customers);

        var viewModel = new CustomerListViewModel(_mockUserRepository.Object);

        // Act
        await Task.Delay(100);

        // Assert
        Assert.Equal(customers.Count, viewModel.Items.Count);
        for (int i = 0; i < customers.Count; i++)
        {
            Assert.Equal(customers[i].Id, viewModel.Items[i].Id);
            Assert.Equal(customers[i].FirstName, viewModel.Items[i].FirstName);
            Assert.Equal(customers[i].LastName, viewModel.Items[i].LastName);
        }
    }

    [Fact]
    public async Task LoadEmployees_ShouldUpdateEmployeesCollection()
    {
        // Arrange
        List<User> employees = new List<User>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe" },
            new() { Id = 2, FirstName = "Jane", LastName = "Doe" }
        };
        _mockUserRepository.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

        var viewModel = new UserListViewModel(_mockUserRepository.Object);

        // Act
        await Task.Delay(100);

        // Assert
        Assert.Equal(employees.Count, viewModel.Items.Count);
        for (int i = 0; i < employees.Count; i++)
        {
            Assert.Equal(employees[i].Id, viewModel.Items[i].Id);
            Assert.Equal(employees[i].FirstName, viewModel.Items[i].FirstName);
            Assert.Equal(employees[i].LastName, viewModel.Items[i].LastName);
        }
    }
}