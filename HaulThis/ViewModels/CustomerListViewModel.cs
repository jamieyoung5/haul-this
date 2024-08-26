using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.ViewModels
{
  public sealed class CustomerListViewModel : ViewModelEvent
  {
    private readonly IManageCustomersService _customerService;

    public ObservableCollection<Customer> Customers { get; private init; } = new ObservableCollection<Customer>();

    public CustomerListViewModel(IManageCustomersService customerService)
    {
      _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
      LoadCustomers();
    }

    public async void LoadCustomers()
    {
      try
      {
        var customersFromDb = await _customerService.GetAllCustomersAsync();
        Customers.Clear();

        foreach (var customer in customersFromDb)
        {
          Customers.Add(customer);
        }

        OnPropertyChanged(nameof(Customers));
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine($"Error loading customers: {ex.Message}");
      }
    }

    public async Task AddCustomerAsync(Customer customer)
    {
      if (customer == null) throw new ArgumentNullException(nameof(customer));

      await _customerService.AddCustomerAsync(customer);
      Customers.Add(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
      if (customer == null) throw new ArgumentNullException(nameof(customer));

      await _customerService.UpdateCustomerAsync(customer);

      var existingCustomer = Customers.FirstOrDefault(c => c.Id == customer.Id);
      if (existingCustomer != null)
      {
        var index = Customers.IndexOf(existingCustomer);
        Customers[index] = customer;
      }
    }

    public async Task DeleteCustomerAsync(Customer customer)
    {
      if (customer == null) throw new ArgumentNullException(nameof(customer));

      var result = await _customerService.DeleteCustomerAsync(customer.Id);
      if (result > 0)
      {
        Customers.Remove(customer);
      }
    }
  }
}
