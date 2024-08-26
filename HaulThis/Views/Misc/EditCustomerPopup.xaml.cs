using HaulThis.Models;
using System;

namespace HaulThis.Views.Misc;

public partial class EditCustomerPopup
{
  private readonly Models.Customer _customer;

  public EditCustomerPopup(Models.Customer customer)
  {
    InitializeComponent();

    _customer = customer;

    FirstNameEntry.Text = customer.FirstName;
    LastNameEntry.Text = customer.LastName;
    EmailEntry.Text = customer.Email;
    PhoneEntry.Text = customer.PhoneNumber;
    AddressEntry.Text = customer.Address;
    CityEntry.Text = customer.City;
    PostalCodeEntry.Text = customer.PostalCode;
    CountryEntry.Text = customer.Country;
    IsActiveSwitch.IsToggled = customer.IsActive;
  }

  private void OnCloseButtonClicked(object sender, EventArgs e)
  {
    Close();
  }

  private void OnSaveButtonClicked(object sender, EventArgs e)
  {
    _customer.FirstName = FirstNameEntry.Text;
    _customer.LastName = LastNameEntry.Text;
    _customer.Email = EmailEntry.Text;
    _customer.PhoneNumber = PhoneEntry.Text;
    _customer.Address = AddressEntry.Text;
    _customer.City = CityEntry.Text;
    _customer.PostalCode = PostalCodeEntry.Text;
    _customer.Country = CountryEntry.Text;
    _customer.IsActive = IsActiveSwitch.IsToggled;

    Close(true);
  }
}

