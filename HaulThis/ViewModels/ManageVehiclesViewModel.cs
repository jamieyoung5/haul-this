using System.Collections.ObjectModel;
using System.ComponentModel;
using HaulThis.Models;
using HaulThis.Services;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HaulThis.Tests")]

namespace HaulThis.ViewModels;

public class ManageVehiclesViewModel : INotifyPropertyChanged
{   
    IManageVehiclesService _ManageVehiclesService;

    public ObservableCollection<Vehicle> vehicles { get; private init; } = new();
    public ManageVehiclesViewModel(IManageVehiclesService manageVehiclesService)
    {
        _ManageVehiclesService = manageVehiclesService;
        LoadVehicles();
    }

    

    internal async Task LoadVehicles()
    {
        var vehiclesFromDb = await _ManageVehiclesService.GetAllVehiclesAsync();
        vehicles.Clear();

        foreach (var vehicle in vehiclesFromDb)
        {
            vehicles.Add(vehicle);
        }

        OnPropertyChanged(nameof(vehicles));
    }

    public event PropertyChangedEventHandler? PropertyChanged;    

    internal void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}
