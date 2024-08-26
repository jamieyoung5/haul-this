using CommunityToolkit.Maui.Views;
using HaulThis.Models;

namespace HaulThis.Views.Misc;

public partial class AddCustomerPopup : Popup
{
  public AddCustomerPopup()
  {
    InitializeComponent();
  }

  private void OnSubmitButtonClicked(object sender, EventArgs e)
  {
    var newCustomer = new Models.Customer
    {
      FirstName = FirstNameEntry.Text,
      LastName = LastNameEntry.Text,
      Email = EmailEntry.Text,
      PhoneNumber = PhoneEntry.Text,
      Address = AddressEntry.Text,
      City = CityEntry.Text,
      PostalCode = PostalCodeEntry.Text,
      Country = CountryEntry.Text,
      IsActive = IsActiveCheckBox.IsChecked
    };

    Close(newCustomer);
  }

  private void OnCloseButtonClicked(object sender, EventArgs e)
  {
    Close();
  }
}

