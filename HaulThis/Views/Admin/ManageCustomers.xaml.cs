using CommunityToolkit.Maui.Views;
using HaulThis.Models;
using HaulThis.Repository;
using HaulThis.Services;
using HaulThis.ViewModels;
using HaulThis.Views.Misc;


namespace HaulThis.Views.Admin;

public partial class ManageCustomers
{
  private readonly IUserRepository _userRepository;

  public ManageCustomers(IUserRepository userRepository)
  {
    InitializeComponent();
    _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    BindingContext = new CustomerListViewModel(_userRepository);
  }

  private async void OnAddButtonClicked(object sender, EventArgs e)
  {
    var addUserPopup = new AddUserPopup();

    object? result = await this.ShowPopupAsync(addUserPopup);

    if (result is not User newUser) return;

    await _userRepository.AddUserAsync(newUser);

    BindingContext = new CustomerListViewModel(_userRepository);
  }

  private async void OnEditButtonClicked(object sender, EventArgs e)
  {
    var button = sender as Button;

    if (button?.CommandParameter is not User userToEdit) return;
    var editUserPopup = new EditUserPopup(userToEdit);
    object? result = await this.ShowPopupAsync(editUserPopup);

    if (result is true)
    {
      await _userRepository.UpdateUserAsync(userToEdit);
    }
  }

  private async void OnDeleteButtonClicked(object sender, EventArgs e)
  {
    var button = sender as Button;

    if (button?.CommandParameter is not User userToDelete) return;
    int result = await _userRepository.DeleteUserAsync(userToDelete.Id);

    if (result <= 0) return;
    var viewModel = BindingContext as CustomerListViewModel;

    viewModel?.Items.Remove(userToDelete);
  }
}