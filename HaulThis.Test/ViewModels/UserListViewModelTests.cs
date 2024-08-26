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
        Assert.NotNull(_viewModel.Employees);
        Assert.IsType<ObservableCollection<User>>(_viewModel.Customers);
    }

  public void Constructor_ShouldCallLoadEmployee()
  {
    _mockUserService.Verify(s => s.GetAllEmployeesAsync(), Times.Once);
  }

  public void Constructor_ShouldCallLoadCustomer()
  {
    _mockUserService.Verify(s => s.GetAllCustomersAsync(), Times.Once);
  }

  [Fact]
  public async Task LoadCustomers_ShouldUpdateCustomersCollection()
  {
    // Arrange
    var customers = new List<User>
        {
            new() { Id = 1, FirstName = "Alice", LastName = "Smith" },
            new() { Id = 2, FirstName = "Bob", LastName = "Johnson" }
        };
    _mockUserService.Setup(s => s.GetAllCustomersAsync()).ReturnsAsync(customers);

    // Act
    var viewModel = new UserListViewModel(_mockUserService.Object);

    // Assert
    Assert.Equal(customers.Count, viewModel.Customers.Count);
      for (int i = 0; i < customers.Count; i++)
      {
        Assert.Equal(customers[i].Id, viewModel.Customers[i].Id);
        Assert.Equal(customers[i].FirstName, viewModel.Customers[i].FirstName);
        Assert.Equal(customers[i].LastName, viewModel.Customers[i].LastName);
      }
  }

  [Fact]
  public async Task LoadEmployees_ShouldUpdateEmployeesCollection()
  {
    // Arrange
    var employees = new List<User>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe" },
            new() { Id = 2, FirstName = "Jane", LastName = "Doe" }
        };
    _mockUserService.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

    // Act
    var viewModel = new UserListViewModel(_mockUserService.Object);

    // Assert
    Assert.Equal(employees.Count, viewModel.Employees.Count);
    for (int i = 0; i < employees.Count; i++)
    {
      Assert.Equal(employees[i].Id, viewModel.Employees[i].Id);
      Assert.Equal(employees[i].FirstName, viewModel.Employees[i].FirstName);
      Assert.Equal(employees[i].LastName, viewModel.Employees[i].LastName);
    }
  }
}