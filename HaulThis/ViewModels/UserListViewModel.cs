using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Repository;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class UserListViewModel(IUserRepository userRepository) 
  : ListViewModel<User, IUserRepository>(userRepository)
{
  protected override async Task<IEnumerable<User>> GetItemsAsync()
  {
    return await _service.GetAllEmployeesAsync();
  }
}

public class CustomerListViewModel(IUserRepository userRepository) 
  : ListViewModel<User, IUserRepository>(userRepository)
{
  protected override async Task<IEnumerable<User>> GetItemsAsync()
  {
    return await _service.GetAllCustomersAsync();
  }
}