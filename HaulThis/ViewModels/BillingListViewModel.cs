using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class BillingListViewModel : ViewModelEvent
{
    private readonly IBillingRepository _billingRepository;
    
    public ObservableCollection<Bill> Bills { get; private set; } = new();

    public BillingListViewModel(IBillingRepository billingRepository)
    {
        _billingRepository = billingRepository;

        LoadBills();
    }

    private async Task LoadBills()
    {
        IEnumerable<Bill> billsFromDb = await _billingRepository.GetBillsByUserAsync(1);
        Bills.Clear();

        foreach (var bill in billsFromDb)
        {
            Bills.Add(bill);
        }
        
        OnPropertyChanged(nameof(Bills));
    }
}