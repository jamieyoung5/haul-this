using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class TripListViewModel : ViewModelEvent
{
    private readonly ITripRepository _tripRepository;

    public ObservableCollection<Trip> Trips { get; init; } = [];

    public TripListViewModel(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
        LoadTrips();
    }

    private async Task LoadTrips()
    {
        var tripsFromDb = await _tripRepository.GetTripByDateAsync(DateTime.Now);
        Trips.Clear();

        foreach (var trip in tripsFromDb)
        {
            Trips.Add(trip);
        }

        OnPropertyChanged(nameof(Trips));
    }
}