using CommunityToolkit.Maui.Views;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;
using HaulThis.Views.Misc;

namespace HaulThis.Views.Admin
{
  public partial class ManageCustomers : ContentPage
  {
    private readonly IManageCustomersService _customerService;

    public ManageCustomers(IManageCustomersService customerService)
    {
      InitializeComponent();
      _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
      BindingContext = new CustomerListViewModel(_customerService);
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
      var addCustomerPopup = new AddCustomerPopup();

      object? result = await this.ShowPopupAsync(addCustomerPopup);

      if (result is not Models.Customer newCustomer) return;

      await _customerService.AddCustomerAsync(newCustomer);

      BindingContext = new CustomerListViewModel(_customerService);
    }

    private async void OnEditButtonClicked(object sender, EventArgs e)
    {
      var button = sender as Button;

      if (button?.CommandParameter is not Models.Customer customerToEdit) return;
      var editCustomerPopup = new EditCustomerPopup(customerToEdit);
      object? result = await this.ShowPopupAsync(editCustomerPopup);

      if (result is true)
      {
        await _customerService.UpdateCustomerAsync(customerToEdit);
      }
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
      var button = sender as Button;

      if (button?.CommandParameter is not Models.Customer customerToDelete) return;
      int result = await _customerService.DeleteCustomerAsync(customerToDelete.Id);

      if (result <= 0) return;

      var viewModel = BindingContext as CustomerListViewModel;

      viewModel?.Customers.Remove(customerToDelete);
    }
  }
}
