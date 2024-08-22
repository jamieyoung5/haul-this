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
        // Create a new user with the entered data
        CreatedUser = new User
        {
            FirstName = FirstNameEntry.Text,
            LastName = LastNameEntry.Text,
            Email = EmailEntry.Text,
            PhoneNumber = PhoneEntry.Text,
            Role = (Role)Enum.Parse(typeof(Role), RolePicker.SelectedItem.ToString())
        };

        // Close the popup and return the created user
        Close(CreatedUser);
    }
}