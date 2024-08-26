using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Views.Driver;

public partial class ManageTrips
{
	private readonly ITripService _tripService;
	
	public ManageTrips(ITripService tripService)
	{
		InitializeComponent();
		_tripService = tripService;
		BindingContext = new TripListViewModel(tripService);
	}

	private async void OnMarkDeliveredButtonClicked(object sender, EventArgs e)
	{
		var button = sender as Button;

		if (button?.CommandParameter is not Delivery deliveryToMark) return;

		int result = await _tripService.MarkItemAsDelivered(deliveryToMark.TripId, deliveryToMark.Id);
        
		if (result <= 0) return;

		var viewModel = BindingContext as TripListViewModel;

		if (viewModel?.Trips == null) return;

		// Locate the trip that contains the delivery item to remove it from the UI
		var trip = viewModel.Trips.FirstOrDefault(t => t.Id == deliveryToMark.TripId);
		if (trip != null)
		{
			trip.TripManifest.Remove(deliveryToMark);

			// Optionally, if the TripManifest is empty, you might want to remove the entire trip
			if (trip.TripManifest.Count == 0)
			{
				viewModel.Trips.Remove(trip);
			}
		}
	}
}