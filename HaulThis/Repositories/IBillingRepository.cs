using HaulThis.Models;

namespace HaulThis.Repositories;

public interface IBillingRepository
{
    Task<IEnumerable<Bill>> GetBillsByUserAsync(int userId);
}