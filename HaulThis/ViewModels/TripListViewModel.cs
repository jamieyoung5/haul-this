using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class TripListViewModel(ITripRepository tripRepository) 
    : ListViewModel<Trip, ITripRepository>(tripRepository)
{
    protected override async Task<IEnumerable<Trip>> GetItemsAsync()
    {
        return await _service.GetTripByDateAsync(DateTime.Now);
    }
}