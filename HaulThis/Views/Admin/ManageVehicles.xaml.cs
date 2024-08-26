using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;
using CommunityToolkit.Maui.Views;
using HaulThis.Views.Misc;

namespace HaulThis.Views.Admin;

public partial class ManageVehicles : ContentPage
{
	private readonly IManageVehiclesService _manageVehiclesService;
	public ManageVehicles(IManageVehiclesService manageVehiclesService)
	{
		InitializeComponent();
		_manageVehiclesService = manageVehiclesService ?? throw new ArgumentNullException(nameof(manageVehiclesService));
		BindingContext = new ManageVehiclesViewModel(_manageVehiclesService);
	}

	private async void OnAddButtonClicked(object sender, EventArgs e)
	{
		var addVehiclePopup = new AddVechiclePopup();
		object? result = await this.ShowPopupAsync(addVehiclePopup);

		if (result is not Vehicle newVehicle) return;

		await _manageVehiclesService.AddVehicleAsync(newVehicle);

		BindingContext = new ManageVehiclesViewModel(_manageVehiclesService);
	}

	private async void OnEditButtonClicked(object sender, EventArgs e)
	{
		var button = sender as Button;

		if (button?.CommandParameter is not Vehicle vehicleToEdit) return;
		var editVehiclePopup = new EditVehiclePopup(vehicleToEdit);
		object? result = await this.ShowPopupAsync(editVehiclePopup);

		if (result is true)
		{
			await _manageVehiclesService.UpdateVehicleAsync(vehicleToEdit);
		}
	}

	private async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		var button = sender as Button;

		if (button?.CommandParameter is not Vehicle vehicleToDelete) return;
		int result = await _manageVehiclesService.DeleteVehicleAsync(vehicleToDelete.Id);

		if (result <= 0) return;
		var viewModel = BindingContext as ManageVehiclesViewModel;

		viewModel?.Vehicles.Remove(vehicleToDelete);
	}

	private async void OnRefreshButtonClicked(object sender, EventArgs e)
	{
		var viewModel = BindingContext as ManageVehiclesViewModel;
		if (viewModel != null)
		{
			await viewModel.RefreshVehicles();
		}
	}
}