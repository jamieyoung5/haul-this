namespace HaulThis.Repositories;

public interface IItemRepository
{
    Task<int> MarkAsDeliveredAsync(int tripId, int itemId);
}