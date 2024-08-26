using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public sealed class UserListViewModel : ViewModelEvent
{
    private readonly IUserService _userService;

    public ObservableCollection<User> Users { get; private init; } = [];

    public UserListViewModel(IUserService userService)
    {
        _userService = userService;
        LoadUsers();
    }
    
    private async Task LoadUsers()
    {
        IEnumerable<User> usersFromDb = await _userService.GetAllUsersAsync();
        Users.Clear();

        foreach (var user in usersFromDb)
        {
            Users.Add(user);
        }

        OnPropertyChanged(nameof(Users));
    }
}