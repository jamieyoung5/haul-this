using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Views.Driver;

public partial class ManageTrips
{
	private readonly IItemRepository _itemRepository;
	
	public ManageTrips(ITripRepository tripRepository, IItemRepository itemRepository)
	{
		InitializeComponent();
		_itemRepository = itemRepository;
		BindingContext = new TripListViewModel(tripRepository);
	}

	private async void OnMarkDeliveredButtonClicked(object sender, EventArgs e)
	{
		var button = sender as Button;

		if (button?.CommandParameter is not Delivery deliveryToMark) return;

		int result = await _itemRepository.MarkAsDeliveredAsync(deliveryToMark.TripId, deliveryToMark.Id);
        
		if (result <= 0) return;

		var viewModel = BindingContext as TripListViewModel;

		if (viewModel?.Items == null) return;
		
		var trip = viewModel.Items.FirstOrDefault(t => t.Id == deliveryToMark.TripId);
		if (trip != null)
		{
			trip.TripManifest.Remove(deliveryToMark);
			
			if (trip.TripManifest.Count == 0)
			{
				viewModel.Items.Remove(trip);
			}
		}
	}
}