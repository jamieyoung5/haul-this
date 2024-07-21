namespace HaulThis.Views.Admin;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();
	}

	private async void ManageEmployees_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ManageEmployees));
	}

	private async void ManageVehicles_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ManageVehicles));
	}

	private async void ManageCustomers_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ManageCustomers));
	}

	private async void ManageCustomerBills_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ManageCustomerBills));
	}

	private async void ManageTripManifests_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ManageTripManifests));
	}

	private async void PlanRouteTrips_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(PlanRouteTrips));
	}

	private async void ResourceTrips_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ResourceTrips));
	}

	private async void TrackTrips_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(TrackTrips));
	}


}