using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Test.ViewModels
{
  public class CustomerListViewModelTests
  {
    private readonly Mock<IManageCustomersService> _mockCustomerService;
    private readonly CustomerListViewModel _viewModel;

    public CustomerListViewModelTests()
    {
      _mockCustomerService = new Mock<IManageCustomersService>();
      _viewModel = new CustomerListViewModel(_mockCustomerService.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeCustomersCollection()
    {
      Assert.NotNull(_viewModel.Customers);
      Assert.IsType<ObservableCollection<Customer>>(_viewModel.Customers);
    }

    [Fact]
    public void Constructor_ShouldCallLoadCustomers()
    {
      _mockCustomerService.Verify(s => s.GetAllCustomersAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadCustomers_ShouldUpdateCustomersCollection()
    {
      // Arrange
      var customers = new List<Customer>
            {
                new() { Id = 1, FirstName = "John", LastName = "Doe" },
                new() { Id = 2, FirstName = "Jane", LastName = "Doe" }
            };
      _mockCustomerService.Setup(s => s.GetAllCustomersAsync()).ReturnsAsync(customers);

      // Act
      _viewModel.LoadCustomers(); // This is now an async method in your actual code

      // Assert
      Assert.Equal(customers.Count, _viewModel.Customers.Count);
      for (int i = 0; i < customers.Count; i++)
      {
        Assert.Equal(customers[i].Id, _viewModel.Customers[i].Id);
        Assert.Equal(customers[i].FirstName, _viewModel.Customers[i].FirstName);
        Assert.Equal(customers[i].LastName, _viewModel.Customers[i].LastName);
      }
    }

    [Fact]
    public async Task AddCustomerAsync_ShouldAddCustomerToCollection()
    {
      // Arrange
      var customer = new Customer
      {
        Id = 1,
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        PhoneNumber = "1234567890",
        Address = "123 Main St",
        City = "City",
        PostalCode = "12345",
        Country = "Country",
        CreatedAt = DateTime.UtcNow,
        IsActive = true
      };

      _mockCustomerService.Setup(s => s.AddCustomerAsync(customer)).ReturnsAsync(1);

      // Act
      await _viewModel.AddCustomerAsync(customer);

      // Assert
      Assert.Contains(customer, _viewModel.Customers);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldModifyCustomerInCollection()
    {
      // Arrange
      var customer = new Customer
      {
        Id = 1,
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        PhoneNumber = "1234567890",
        Address = "123 Main St",
        City = "City",
        PostalCode = "12345",
        Country = "Country",
        CreatedAt = DateTime.UtcNow,
        IsActive = true
      };

      var updatedCustomer = new Customer
      {
        Id = 1,
        FirstName = "John",
        LastName = "Smith", // Updated last name
        Email = "john.doe@example.com",
        PhoneNumber = "1234567890",
        Address = "123 Main St",
        City = "City",
        PostalCode = "12345",
        Country = "Country",
        CreatedAt = DateTime.UtcNow,
        IsActive = true
      };

      _mockCustomerService.Setup(s => s.UpdateCustomerAsync(updatedCustomer)).ReturnsAsync(1);
      _viewModel.Customers.Add(customer); // Add original customer to collection

      // Act
      await _viewModel.UpdateCustomerAsync(updatedCustomer);

      // Assert
      var updatedCustomerInCollection = _viewModel.Customers.FirstOrDefault(c => c.Id == updatedCustomer.Id);
      Assert.NotNull(updatedCustomerInCollection);
      Assert.Equal("Smith", updatedCustomerInCollection.LastName);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldRemoveCustomerFromCollection()
    {
      // Arrange
      var customer = new Customer
      {
        Id = 1,
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        PhoneNumber = "1234567890",
        Address = "123 Main St",
        City = "City",
        PostalCode = "12345",
        Country = "Country",
        CreatedAt = DateTime.UtcNow,
        IsActive = true
      };

      _mockCustomerService.Setup(s => s.DeleteCustomerAsync(customer.Id)).ReturnsAsync(1);
      _viewModel.Customers.Add(customer); // Add customer to collection

      // Act
      await _viewModel.DeleteCustomerAsync(customer);

      // Assert
      Assert.DoesNotContain(customer, _viewModel.Customers);
    }
  }
}
