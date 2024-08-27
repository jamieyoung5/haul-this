using HaulThis.Models;

namespace HaulThis.Repositories;

public interface IBillingRepository
{
    /// <summary>
    /// Gets all bills associated with a user
    /// </summary>
    /// <param name="userId">The id of the user to retrieve bills for</param>
    /// <returns>an enumerable collection of bills</returns>
    Task<IEnumerable<Bill>> GetBillsByUserAsync(int userId);
}