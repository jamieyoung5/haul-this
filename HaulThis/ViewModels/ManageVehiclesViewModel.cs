using System.Collections.ObjectModel;
using System.ComponentModel;
using HaulThis.Models;
using HaulThis.Services;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HaulThis.Tests")]

namespace HaulThis.ViewModels;

public class ManageVehiclesViewModel : INotifyPropertyChanged
{   
    readonly IManageVehiclesService _ManageVehiclesService;

    public ObservableCollection<Vehicle> Vehicles { get; private init; } = new();
    public ManageVehiclesViewModel(IManageVehiclesService manageVehiclesService)
    {
        _ManageVehiclesService = manageVehiclesService;
        LoadVehicles();
    }

    internal async Task LoadVehicles()
    {
        var vehiclesFromDb = await _ManageVehiclesService.GetAllVehiclesAsync();
        Vehicles.Clear();

        foreach (var vehicle in vehiclesFromDb)
        {
            Vehicles.Add(vehicle);
        }

        OnPropertyChanged(nameof(Vehicles));
    }
     public async Task RefreshVehicles()
    {
        await LoadVehicles();
    }

    public event PropertyChangedEventHandler? PropertyChanged;    

    internal void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}
