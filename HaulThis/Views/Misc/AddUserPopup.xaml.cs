using HaulThis.Models;


namespace HaulThis.Views.Misc;

public partial class AddUserPopup
{
    private User CreatedUser { get; set; }
        
    public AddUserPopup()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        CreatedUser = new User
        {
            FirstName = FirstNameEntry.Text,
            LastName = LastNameEntry.Text,
            Email = EmailEntry.Text,
            PhoneNumber = PhoneEntry.Text,
            Address = AddressEntry.Text,
            Role = (Role)Enum.Parse(typeof(Role), RolePicker.SelectedItem.ToString())
        };
        
        Close(CreatedUser);
    }
}