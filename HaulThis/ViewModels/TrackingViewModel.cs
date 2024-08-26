using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HaulThis.Services;

namespace HaulThis.ViewModels;

public class TrackingViewModel : INotifyPropertyChanged
{
    private readonly ITrackingService _trackingService;

    public TrackingViewModel(ITrackingService trackingService)
    {
        _trackingService = trackingService;
        TrackItemCommand = new Command(async () => await TrackItem());
    }

    private int _trackingId;
    public int TrackingId
    {
        get => _trackingId;
        set
        {
            _trackingId = value;
            OnPropertyChanged();
        }
    }

    private string _currentLocation;
    public string CurrentLocation
    {
        get => _currentLocation;
        set
        {
            _currentLocation = value;
            OnPropertyChanged();
        }
    }

    private DateTime? _eta;
    public DateTime? ETA
    {
        get => _eta;
        set
        {
            _eta = value;
            OnPropertyChanged();
        }
    }

    private string _status;
    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged();
        }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand TrackItemCommand { get; }

    private async Task TrackItem()
    {
        try
        {
            var trackingInfo = await _trackingService.GetTrackingInfo(TrackingId);

            CurrentLocation = trackingInfo.CurrentLocation;
            ETA = trackingInfo.ETA;
            Status = trackingInfo.Status;
            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
            ClearTrackingData();
        }
    }

    private void ClearTrackingData()
    {
        CurrentLocation = string.Empty;
        ETA = null;
        Status = string.Empty;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}