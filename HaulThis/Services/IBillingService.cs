using HaulThis.Models;

namespace HaulThis.Services;

public interface IBillingService
{
    /// <summary>
    /// Retrieves billing information for the given user id.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve bills for.</param>
    /// <returns>A collection of all bills assigned to the given user id </returns>
    Task<IEnumerable<Bill>> GetBillingInfoByUserAsync(int userId);
}