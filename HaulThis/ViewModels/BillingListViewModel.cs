using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class BillingListViewModel(IBillingRepository billingRepository)
    : ListViewModel<Bill, IBillingRepository>(billingRepository)
{
    protected override async Task<IEnumerable<Bill>> GetItemsAsync()
    {
        return await _service.GetBillsByUserAsync(1);
    }
}