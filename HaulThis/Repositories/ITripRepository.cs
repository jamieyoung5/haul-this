using HaulThis.Models;

namespace HaulThis.Repositories;

public interface ITripRepository
{
    Task<IEnumerable<Trip>> GetTripByDateAsync(DateTime date);
}