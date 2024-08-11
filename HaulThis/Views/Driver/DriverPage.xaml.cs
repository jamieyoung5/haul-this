namespace HaulThis.Views.Driver;

public partial class DriverPage : ContentPage
{
	public DriverPage()
	{
		InitializeComponent();
	}

	private async void ManageTrips_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ManageTrips));
    }

private async void RecordExpenses_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RecordExpenses));
    }

private async void DriverConfirmPickup_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(DriverConfirmPickup));
    }

private async void ReportDelays_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReportDelays));
    }

}