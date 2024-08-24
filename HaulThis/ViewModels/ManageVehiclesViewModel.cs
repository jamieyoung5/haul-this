using System.Collections.ObjectModel;
using System.ComponentModel;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class ManageVehiclesViewModel
{   
    IManageVehiclesService _ManageVehiclesService;

    public ObservableCollection<Vehicle> vehicles { get; private init; } = new();
    public ManageVehiclesViewModel(ManageVehiclesService manageVehiclesService)
    {
        _ManageVehiclesService = manageVehiclesService;
        LoadVehicles();
    }

    private async Task LoadVehicles()
    {
        var vehiclesFromDb = await _ManageVehiclesService.GetAllVehiclesAsync();
        vehicles.Clear();

        foreach (var vehicle in vehiclesFromDb)
        {
            vehicles.Add(vehicle);
        }

        OnPropertyChanged(nameof(vehicles));
    }

    public event PropertyChangingEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}
