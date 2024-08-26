using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HaulThis.Models;
using HaulThis.Services;

[assembly: InternalsVisibleTo("HaulThis.Tests")]
namespace HaulThis.ViewModels
{
  public class ManageReportsViewModel : INotifyPropertyChanged
  {
    private readonly IReportEmergencyService _reportEmergencyService;

    public ObservableCollection<Trip> Trips { get; private init; } = new();
    public ObservableCollection<Report> Reports { get; private init; } = new();

    public ManageReportsViewModel(IReportEmergencyService reportEmergencyService)
    {
      _reportEmergencyService = reportEmergencyService;
      UpdateTripTableCommand = new Command(async () => await LoadTripsFromDriver());
      SelectTripCommand = new Command(async () => await LoadTripReports());
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
      if (int.TryParse(DriverId, out int driverId))
      {
        var tripsFromDb = await _reportEmergencyService.GetTripsByDriverIdAsync(driverId);
        Trips.Clear();

        foreach (var trip in tripsFromDb)
        {
          Trips.Add(trip);
        }

        OnPropertyChanged(nameof(Trips));
      }
      else
      {
        // Handle invalid driver ID (e.g., show an error message)
      }
    }

    public ICommand SelectTripCommand { get; }
    internal async Task LoadTripReports()
    {
      if (SelectedTrip != null)
      {
        var reportsFromDb = await _reportEmergencyService.GetReportsByTripIdAsync(SelectedTrip.Id);
        Reports.Clear();

        foreach (var report in reportsFromDb)
        {
          Reports.Add(report);
        }

        OnPropertyChanged(nameof(Reports));
      }
    }

    public async Task RefreshReports()
    {
      await LoadTripReports();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
