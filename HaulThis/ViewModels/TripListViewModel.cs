using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class TripListViewModel : ViewModelEvent
{
    private readonly ITripService _tripService;

    public ObservableCollection<Trip> Trips { get; init; } = [];

    public TripListViewModel(ITripService tripService)
    {
        _tripService = tripService;
        LoadTrips();
    }

    private async Task LoadTrips()
    {
        var tripsFromDb = await _tripService.GetTripByDateAsync(DateTime.Now);
        Trips.Clear();

        foreach (var trip in tripsFromDb)
        {
            Trips.Add(trip);
        }

        OnPropertyChanged(nameof(Trips));
    }
}