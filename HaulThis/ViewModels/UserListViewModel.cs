using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public sealed class UserListViewModel : ViewModelEvent
{
    private readonly IUserService _userService;

    public ObservableCollection<User> Employees { get; private set; } = new();
    public ObservableCollection<User> Customers { get; private set; } = new();

  public UserListViewModel(IUserService userService)
    {
        _userService = userService;
        LoadEmployees();
        LoadCustomers();
    }

    private async Task LoadEmployees()
    {
      IEnumerable<User> employeesFromDb = await _userService.GetAllEmployeesAsync();
      Employees.Clear();

      foreach (var employee in employeesFromDb)
      {
        Employees.Add(employee);
      }

      OnPropertyChanged(nameof(Employees));
    }

    private async Task LoadCustomers()
    {
      IEnumerable<User> customersFromDb = await _userService.GetAllCustomersAsync();
      Customers.Clear();

      foreach (var customer in customersFromDb)
      {
      Customers.Add(customer);
      }

      OnPropertyChanged(nameof(Customers));
    }
}