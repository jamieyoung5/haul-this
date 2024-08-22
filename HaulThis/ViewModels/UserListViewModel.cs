using System.Collections.ObjectModel;
using System.ComponentModel;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public sealed class UserListViewModel : INotifyPropertyChanged
{
    private readonly IUserService _userService;

    public ObservableCollection<User> Users { get; private init; } = new();

    public UserListViewModel(IUserService userService)
    {
        _userService = userService;
        LoadUsers();
    }

    private async void LoadUsers()
    {
        var usersFromDb = await _userService.GetAllUsersAsync();
        Users.Clear();

        foreach (var user in usersFromDb)
        {
            Users.Add(user);
        }

        OnPropertyChanged(nameof(Users));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}