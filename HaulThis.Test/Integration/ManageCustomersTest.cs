using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.Test.Integration
{
  public class ManageCustomersServiceTests : DisposableIntegrationTest
  {
    private readonly IManageCustomersService _customerService;

    public ManageCustomersServiceTests()
    {
      _customerService = new ManageCustomersService(_databaseService);
    }

    [Fact]
    public async Task AddCustomer_ShouldAddCustomerToDatabase_WhenValidCustomerIsProvided()
    {
      // Arrange
      var customer = new Customer
      {
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

      // Act
      int result = await _customerService.AddCustomerAsync(customer);

      // Assert
      var customers = await _customerService.GetAllCustomersAsync();
      Assert.Equal(1, result);
      Assert.Contains(customers, c => c.Email == "john.doe@example.com");
    }

    [Fact]
    public async Task DeleteCustomer_ShouldRemoveCustomerFromDatabase_WhenCustomerIdExists()
    {
      // Arrange
      var customer = new Customer
      {
        FirstName = "Jane",
        LastName = "Doe",
        Email = "jane.doe@example.com",
        PhoneNumber = "0987654321",
        Address = "456 Another St",
        City = "Another City",
        PostalCode = "67890",
        Country = "Another Country",
        CreatedAt = DateTime.UtcNow,
        IsActive = true
      };

      int customerId = await _customerService.AddCustomerAsync(customer);
      var customersBeforeDeletion = await _customerService.GetAllCustomersAsync();
      Assert.Contains(customersBeforeDeletion, c => c.Email == "jane.doe@example.com");

      // Act
      int result = await _customerService.DeleteCustomerAsync(customerId);

      // Assert
      var customersAfterDeletion = await _customerService.GetAllCustomersAsync();
      Assert.Equal(1, result);
      Assert.DoesNotContain(customersAfterDeletion, c => c.Email == "jane.doe@example.com");
    }

    [Fact]
    public async Task UpdateCustomer_ShouldModifyCustomerInDatabase_WhenEditsAreValid()
    {
      // Arrange
      var customer = new Customer
      {
        FirstName = "Mark",
        LastName = "Smith",
        Email = "mark.smith@example.com",
        PhoneNumber = "5551234567",
        Address = "789 New St",
        City = "New City",
        PostalCode = "98765",
        Country = "New Country",
        CreatedAt = DateTime.UtcNow,
        IsActive = true
      };

      int customerId = await _customerService.AddCustomerAsync(customer);
      var customerToUpdate = await _customerService.GetCustomerByIdAsync(customerId);
      customerToUpdate.LastName = "UpdatedSmith";

      // Act
      int result = await _customerService.UpdateCustomerAsync(customerToUpdate);

      // Assert
      var updatedCustomer = await _customerService.GetCustomerByIdAsync(customerId);
      Assert.Equal(1, result);
      Assert.Equal("UpdatedSmith", updatedCustomer.LastName);
    }
  }
}
