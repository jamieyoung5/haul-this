using HaulThis.Models;

namespace HaulThis.Views.Misc;

public partial class EditUserPopup
{
    private readonly User _user;
    
    public EditUserPopup(User user)
    {
        InitializeComponent();

        _user = user;
        
        FirstNameEntry.Text = user.FirstName;
        LastNameEntry.Text = user.LastName;
        EmailEntry.Text = user.Email;
        PhoneEntry.Text = user.PhoneNumber;
        RolePicker.SelectedItem = user.Role.ToString();
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        _user.FirstName = FirstNameEntry.Text;
        _user.LastName = LastNameEntry.Text;
        _user.Email = EmailEntry.Text;
        _user.PhoneNumber = PhoneEntry.Text;
        _user.Address = AddressEntry.Text;
        _user.Role = (Role)Enum.Parse(typeof(Role), RolePicker.SelectedItem.ToString() ?? "UNKNOWN");
        
        Close(true);
    }
}