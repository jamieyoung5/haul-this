using HaulThis.Services;

namespace HaulThis.Repositories;

public class ItemRepository(IDatabaseService databaseService) : IItemRepository
{
    public async Task<int> MarkAsDeliveredAsync(int tripId, int itemId)
    {
        const string markItemDeliveredStmt = """
                                             UPDATE item
                                             SET delivered = 1
                                             WHERE id = @p0 AND tripId = @p1;
                                             """;

        return await Task.FromResult(databaseService.Execute(markItemDeliveredStmt, itemId, tripId));
    }
}