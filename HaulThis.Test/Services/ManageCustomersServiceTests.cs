using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.Test.Services;

public class ManageCustomersServiceTests
{
  private readonly Mock<IDatabaseService> _mockDatabaseService;
  private readonly ManageCustomersService _customerService;

  public ManageCustomersServiceTests()
  {
    _mockDatabaseService = new Mock<IDatabaseService>();
    _customerService = new ManageCustomersService(_mockDatabaseService.Object);
  }

  [Fact]
  public async Task GetAllCustomersAsync_ReturnListOfCustomers()
  {
    // Arrange
    var dataTable = new DataTable();
    dataTable.Columns.Add("Id", typeof(int));
    dataTable.Columns.Add("FirstName", typeof(string));
    dataTable.Columns.Add("LastName", typeof(string));
    dataTable.Columns.Add("Email", typeof(string));
    dataTable.Columns.Add("PhoneNumber", typeof(string));
    dataTable.Columns.Add("Address", typeof(string));
    dataTable.Columns.Add("City", typeof(string));
    dataTable.Columns.Add("PostalCode", typeof(string));
    dataTable.Columns.Add("Country", typeof(string));
    dataTable.Columns.Add("CreatedAt", typeof(DateTime));
    dataTable.Columns.Add("UpdatedAt", typeof(DateTime));
    dataTable.Columns.Add("IsActive", typeof(bool));

    var row = dataTable.NewRow();
    row["Id"] = 1;
    row["FirstName"] = "John";
    row["LastName"] = "Doe";
    row["Email"] = "john.doe@example.com";
    row["PhoneNumber"] = "1234567890";
    row["Address"] = "123 Main St";
    row["City"] = "City";
    row["PostalCode"] = "12345";
    row["Country"] = "Country";
    row["CreatedAt"] = DateTime.UtcNow;
    row["UpdatedAt"] = DBNull.Value;
    row["IsActive"] = true;
    dataTable.Rows.Add(row);

    _mockDatabaseService.Setup(d => d.Query(It.IsAny<string>())).Returns(dataTable.CreateDataReader());

    // Act
    var customers = await _customerService.GetAllCustomersAsync();

    // Assert
    Assert.Single(customers);
    Assert.Equal("John", customers.First().FirstName);
  }

  [Fact]
  public async Task GetCustomerByIdAsync_ReturnTheCustomer_WhenTheCustomerExists()
  {
    // Arrange
    var dataTable = new DataTable();
    dataTable.Columns.Add("Id", typeof(int));
    dataTable.Columns.Add("FirstName", typeof(string));
    dataTable.Columns.Add("LastName", typeof(string));
    dataTable.Columns.Add("Email", typeof(string));
    dataTable.Columns.Add("PhoneNumber", typeof(string));
    dataTable.Columns.Add("Address", typeof(string));
    dataTable.Columns.Add("City", typeof(string));
    dataTable.Columns.Add("PostalCode", typeof(string));
    dataTable.Columns.Add("Country", typeof(string));
    dataTable.Columns.Add("CreatedAt", typeof(DateTime));
    dataTable.Columns.Add("UpdatedAt", typeof(DateTime));
    dataTable.Columns.Add("IsActive", typeof(bool));

    var row = dataTable.NewRow();
    row["Id"] = 1;
    row["FirstName"] = "John";
    row["LastName"] = "Doe";
    row["Email"] = "john.doe@example.com";
    row["PhoneNumber"] = "1234567890";
    row["Address"] = "123 Main St";
    row["City"] = "City";
    row["PostalCode"] = "12345";
    row["Country"] = "Country";
    row["CreatedAt"] = DateTime.UtcNow;
    row["UpdatedAt"] = DBNull.Value;
    row["IsActive"] = true;
    dataTable.Rows.Add(row);

    _mockDatabaseService.Setup(d => d.QueryRow(It.IsAny<string>(), It.IsAny<int>())).Returns(dataTable.CreateDataReader());

    // Act
    var customer = await _customerService.GetCustomerByIdAsync(1);

    // Assert
    Assert.Equal(1, customer.Id);
    Assert.Equal("John", customer.FirstName);
  }

  [Fact]
  public async Task AddCustomerAsync_ReturnNumberOfRowsAffected()
  {
    // Arrange
    _mockDatabaseService.Setup(d => d.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
        .Returns(1);

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

    // Act
    var result = await _customerService.AddCustomerAsync(customer);

    // Assert
    Assert.Equal(1, result);
  }

  [Fact]
  public async Task UpdateCustomerAsync_ReturnNumberOfRowsAffected()
  {
    // Arrange
    _mockDatabaseService.Setup(d => d.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
        .Returns(1);

    var customer = new Customer
    {
      Id = 1,
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

    // Act
    var result = await _customerService.UpdateCustomerAsync(customer);

    // Assert
    Assert.Equal(1, result);
  }

  [Fact]
  public async Task DeleteCustomerAsync_ReturnNumberOfRowsAffected()
  {
    // Arrange
    _mockDatabaseService.Setup(d => d.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
        .Returns(1);

    // Act
    var result = await _customerService.DeleteCustomerAsync(1);

    // Assert
    Assert.Equal(1, result);
  }
}

