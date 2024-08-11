namespace HaulThis.Views.Customer;

public partial class CustomerPage : ContentPage
{
	public CustomerPage()
	{
		InitializeComponent();
	}


	 private async void ManageAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ManageAccount));
    }

	private async void ManageBilling_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ManageBilling));
    }
	
	private async void RequestPickup_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RequestPickup));
    }

	private async void TrackItem_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TrackItem));
    }

	private async void CustomerConfirmPickup_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CustomerConfirmPickup));
    }
}