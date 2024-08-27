namespace HaulThis.Repositories;

public interface IItemRepository
{
    /// <summary>
    /// Marks specified item from specified trip as delivered
    /// </summary>
    /// <param name="tripId">the id of the trip the item is apart of</param>
    /// <param name="itemId">the id of the item</param>
    /// <returns>the number of rows affected</returns>
    Task<int> MarkAsDeliveredAsync(int tripId, int itemId);
}