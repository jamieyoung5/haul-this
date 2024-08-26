using HaulThis.Models;

namespace HaulThis.Services;

public interface ITripService
{
    /// <summary>
    /// Retrieves trips corresponding to the provided date.
    /// </summary>
    /// <param name="date">The date of trips to be retrieved</param>
    /// <returns>A collection of trips corresponding to the specified date.</returns>
    Task<IEnumerable<Trip>> GetTripByDateAsync(DateTime date);
    
    /// <summary>
    /// Marks an item in a trip as delivered
    /// </summary>
    /// <param name="tripId">The id of the trip delivering the item</param>
    /// <param name="itemId">The id of item to be marked as delivered</param>
    Task<int> MarkItemAsDelivered(int tripId, int itemId);
}