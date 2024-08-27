using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class BillingListViewModel : ViewModelEvent
{
    private readonly IBillingService _billingService;
    
    public ObservableCollection<Bill> Bills { get; private set; } = new();

    public BillingListViewModel(IBillingService billingService)
    {
        _billingService = billingService;

        LoadBills();
    }

    private async Task LoadBills()
    {
        IEnumerable<Bill> billsFromDb = await _billingService.GetBillingInfoByUserAsync(1);
        Bills.Clear();

        foreach (var bill in billsFromDb)
        {
            Bills.Add(bill);
        }
        
        OnPropertyChanged(nameof(Bills));
    }
}