using HaulThis.Models;

namespace HaulThis.Views.Misc;

public partial class AddVechiclePopup
{
	private Vehicle newVehicle {get; set;}
	public AddVechiclePopup()
	{
		InitializeComponent();
	}

	private void OnCloseButtonClicked(object sender, EventArgs e)
	{
		Close();
	}

	private void OnSubmitButtonClicked(object sender, EventArgs e)
	{
		var status = StatusPicker.SelectedItem.ToString();
		status = status.Replace(" ", "");

		var newVehicle = new Vehicle
		{
			Make = MakeEntry.Text,
			Model = ModelEntry.Text,
			Year = int.Parse(YearEntry.Text),
			LicensePlate = LicensePlateEntry.Text,
			Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), status)
		};
		
		Close(newVehicle);
	}
}
