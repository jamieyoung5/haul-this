using HaulThis.Models;

namespace HaulThis.Repositories;

public interface ITripRepository
{
    /// <summary>
    /// Retrieves a collection of trips scheduled on a specific date.
    /// </summary>
    /// <param name="date">The date for which trips should be retrieved.</param>
    /// <returns>an enumerable collection of trips</returns>
    Task<IEnumerable<Trip>> GetTripByDateAsync(DateTime date);
}