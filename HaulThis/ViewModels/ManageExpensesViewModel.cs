using System.Collections.ObjectModel;
using System.ComponentModel;
using HaulThis.Models;
using HaulThis.Services;
using System.Runtime.CompilerServices;
using System.Windows.Input;

[assembly: InternalsVisibleTo("HaulThis.Tests")]
namespace HaulThis.ViewModels;

public class ManageExpensesViewModel : INotifyPropertyChanged
{
    readonly IManageExpensesService _ManageExpensesService;

    public ObservableCollection<Trip> Trips { get; private init; } = new();
    public ObservableCollection<Expense> Expenses { get; private init; } = new();
    

    public ManageExpensesViewModel(IManageExpensesService manageExpensesService)
    {
        _ManageExpensesService = manageExpensesService;
        UpdateTripTableCommand = new Command(async () => await LoadTripsFromDriver());
        SelectTripCommand = new Command(async () => await LoadTripExpenses());
    }

    private string _driverId;

    public string DriverId
    {
        get => _driverId;
        set
        {
            _driverId = value;
            OnPropertyChanged();
        }
    }

    private Trip _selectedTrip;
    public Trip SelectedTrip
    {
        get => _selectedTrip;
        set
        {
            _selectedTrip = value;
            OnPropertyChanged();
        }
    }

    public ICommand UpdateTripTableCommand { get; }
    internal async Task LoadTripsFromDriver()
    {
        var tripsFromDb = await _ManageExpensesService.GetTripsByDriverIdAsync(int.Parse(DriverId));
        Trips.Clear();

        foreach (var trip in tripsFromDb)
        {
            Trips.Add(trip);
        }

        OnPropertyChanged(nameof(Trips));
    }

    public ICommand SelectTripCommand { get; }
    internal async Task LoadTripExpenses()
    {
        var expensesFromDb = await _ManageExpensesService.GetExpensesByTripIdAsync(SelectedTrip.Id);
        Expenses.Clear();

        foreach (var expense in expensesFromDb)
        {
            Expenses.Add(expense);
        }

        OnPropertyChanged(nameof(Expenses));
    }

    public async Task RefreshExpenses()
    {
        await LoadTripExpenses();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    internal void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
