using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Repository;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public sealed class UserListViewModel : ViewModelEvent
{
    private readonly IUserRepository _userRepository;

    public ObservableCollection<User> Employees { get; private set; } = new();
    public ObservableCollection<User> Customers { get; private set; } = new();

  public UserListViewModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        LoadEmployees();
        LoadCustomers();
    }

    private async Task LoadEmployees()
    {
      IEnumerable<User> employeesFromDb = await _userRepository.GetAllEmployeesAsync();
      Employees.Clear();

      foreach (var employee in employeesFromDb)
      {
        Employees.Add(employee);
      }

      OnPropertyChanged(nameof(Employees));
    }

    private async Task LoadCustomers()
    {
      IEnumerable<User> customersFromDb = await _userRepository.GetAllCustomersAsync();
      Customers.Clear();

      foreach (var customer in customersFromDb)
      {
      Customers.Add(customer);
      }

      OnPropertyChanged(nameof(Customers));
    }
}