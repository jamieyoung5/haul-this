using CommunityToolkit.Maui.Views;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;
using HaulThis.Views.Misc;


namespace HaulThis.Views.Admin;

public partial class ManageEmployees
{
    private readonly IUserService _userService;

    public ManageEmployees(IUserService userService)
    {
        InitializeComponent();
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        BindingContext = new UserListViewModel(_userService);
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var addUserPopup = new AddUserPopup();

        object? result = await this.ShowPopupAsync(addUserPopup);

        if (result is not User newUser) return;
        
        await _userService.AddUserAsync(newUser);

        BindingContext = new UserListViewModel(_userService);
    }

    private async void OnEditButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button?.CommandParameter is not User userToEdit) return;
        var editUserPopup = new EditUserPopup(userToEdit);
        object? result = await this.ShowPopupAsync(editUserPopup);

        if (result is true)
        {
            await _userService.UpdateUserAsync(userToEdit);
        }
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button?.CommandParameter is not User userToDelete) return;
        int result = await _userService.DeleteUserAsync(userToDelete.Id);
            
        if (result <= 0) return;
        var viewModel = BindingContext as UserListViewModel;
            
        viewModel?.Users.Remove(userToDelete);
    }
}