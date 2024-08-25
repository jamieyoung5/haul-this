using HaulThis.Models;

namespace HaulThis.Views.Misc;

public partial class EditVehiclePopup
{
	private readonly Vehicle _vehicle;
	public EditVehiclePopup(Vehicle vehicle)
	{
		InitializeComponent();

		_vehicle = vehicle;
		MakeEntry.Text = vehicle.Make;
		ModelEntry.Text = vehicle.Model;
		YearEntry.Text = vehicle.Year.ToString();
		LicensePlateEntry.Text = vehicle.LicensePlate;
		StatusPicker.SelectedItem = vehicle.Status.ToString();

	}

	private void OnCloseButtonClicked(object sender, EventArgs e)
	{
		Close();
	}

	private void OnSaveButtonClicked(object sender, EventArgs e)
	{
		var status = StatusPicker.SelectedItem.ToString();
		status = status.Replace(" ", "");

		_vehicle.Make = MakeEntry.Text;
		_vehicle.Model = ModelEntry.Text;
		_vehicle.Year = int.Parse(YearEntry.Text);
		_vehicle.LicensePlate = LicensePlateEntry.Text;
		_vehicle.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), status);
		
		Close(true);
	}

}