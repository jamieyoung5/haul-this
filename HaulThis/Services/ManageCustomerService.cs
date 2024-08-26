using HaulThis.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaulThis.Services
{
  /// <summary>
  /// Service class for managing customers.
  /// </summary>
  public class ManageCustomersService : IManageCustomersService
  {
    private readonly IDatabaseService _databaseService;

    private const string GetAllCustomersQuery = "SELECT Id, FirstName, LastName, Email, PhoneNumber, Address, City, PostalCode, Country, CreatedAt, UpdatedAt, IsActive FROM Customers";
    private const string GetCustomerByIdQuery = "SELECT Id, FirstName, LastName, Email, PhoneNumber, Address, City, PostalCode, Country, CreatedAt, UpdatedAt, IsActive FROM Customers WHERE Id = @p0";

    private const string AddCustomerStmt = """
            INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber, Address, City, PostalCode, Country, CreatedAt, IsActive)
            VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)
        """;

    private const string UpdateCustomerStmt = """
            UPDATE Customers 
            SET FirstName = @p0, LastName = @p1, Email = @p2, PhoneNumber = @p3, Address = @p4, City = @p5, PostalCode = @p6, Country = @p7, UpdatedAt = @p8, IsActive = @p9
            WHERE Id = @p10
        """;

    private const string DeleteCustomerStmt = "DELETE FROM Customers WHERE Id = @p0";

    public ManageCustomersService(IDatabaseService databaseService)
    {
      _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
      var customers = new List<Customer>();

      using (var reader = _databaseService.Query(GetAllCustomersQuery))
      {
        while (reader.Read())
        {
          customers.Add(new Customer
          {
            Id = reader.GetInt32(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            Email = reader.GetString(3),
            PhoneNumber = reader.GetString(4),
            Address = reader.GetString(5),
            City = reader.GetString(6),
            PostalCode = reader.GetString(7),
            Country = reader.GetString(8),
            CreatedAt = reader.GetDateTime(9),
            UpdatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
            IsActive = reader.GetBoolean(11)
          });
        }
      }

      return await Task.FromResult(customers);
    }

    public async Task<Customer> GetCustomerByIdAsync(int customerId)
    {
      var reader = _databaseService.QueryRow(GetCustomerByIdQuery, customerId);

      if (reader.IsDBNull(0))
      {
        throw new InvalidOperationException("Customer not found");
      }

      var customer = new Customer
      {
        Id = reader.GetInt32(0),
        FirstName = reader.GetString(1),
        LastName = reader.GetString(2),
        Email = reader.GetString(3),
        PhoneNumber = reader.GetString(4),
        Address = reader.GetString(5),
        City = reader.GetString(6),
        PostalCode = reader.GetString(7),
        Country = reader.GetString(8),
        CreatedAt = reader.GetDateTime(9),
        UpdatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
        IsActive = reader.GetBoolean(11)
      };

      return await Task.FromResult(customer);
    }

    public async Task<int> AddCustomerAsync(Customer customer)
    {
      return await Task.FromResult(_databaseService.Execute(
          AddCustomerStmt,
          customer.FirstName,
          customer.LastName,
          customer.Email,
          customer.PhoneNumber,
          customer.Address,
          customer.City,
          customer.PostalCode,
          customer.Country,
          customer.CreatedAt,
          customer.IsActive
      ));
    }

    public async Task<int> UpdateCustomerAsync(Customer customer)
    {
      return await Task.FromResult(_databaseService.Execute(
          UpdateCustomerStmt,
          customer.FirstName,
          customer.LastName,
          customer.Email,
          customer.PhoneNumber,
          customer.Address,
          customer.City,
          customer.PostalCode,
          customer.Country,
          DateTime.UtcNow,
          customer.IsActive,
          customer.Id
      ));
    }

    public async Task<int> DeleteCustomerAsync(int customerId)
    {
      return await Task.FromResult(_databaseService.Execute(DeleteCustomerStmt, customerId));
    }
  }
}
