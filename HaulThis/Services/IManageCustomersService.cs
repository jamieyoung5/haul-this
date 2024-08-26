using HaulThis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaulThis.Services
{
  /// <summary>
  /// Interface defining Customer CRUD operations.
  /// </summary>
  public interface IManageCustomersService
  {
    /// <summary>
    /// Retrieves all customers asynchronously.
    /// </summary>
    /// <returns>A collection of all customers.</returns>
    Task<IEnumerable<Customer>> GetAllCustomersAsync();

    /// <summary>
    /// Retrieves a specific customer by their ID asynchronously.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve.</param>
    /// <returns>The customer with the specified ID.</returns>
    Task<Customer> GetCustomerByIdAsync(int customerId);

    /// <summary>
    /// Adds a new customer asynchronously.
    /// </summary>
    /// <param name="customer">The customer to add.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> AddCustomerAsync(Customer customer);

    /// <summary>
    /// Updates an existing customer asynchronously.
    /// </summary>
    /// <param name="customer">The customer to update.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> UpdateCustomerAsync(Customer customer);

    /// <summary>
    /// Deletes a customer by ID asynchronously.
    /// </summary>
    /// <param name="customerId">The ID of the customer to delete.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> DeleteCustomerAsync(int customerId);
  }
}
