using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HaulThis.Models;
using HaulThis.Services;
using Microsoft.Extensions.Logging;

namespace HaulThis.ViewModels
{
  public class PickupRequestModel : INotifyPropertyChanged
  {
    private readonly IPickupRequestService _pickupRequestService;

    public PickupRequestModel(IPickupRequestService pickupRequestService)
    {
      _pickupRequestService = pickupRequestService;
      SubmitPickupRequestCommand = new Command(async () => await SubmitPickupRequest());
    }

    private string _pickupLocation;
    public string PickupLocation
    {
      get => _pickupLocation;
      set
      {
        _pickupLocation = value;
        OnPropertyChanged();
      }
    }

    private string _destination;
    public string Destination
    {
      get => _destination;
      set
      {
        _destination = value;
        OnPropertyChanged();
      }
    }

    private DateTime _requestedTime;
    public DateTime RequestedTime
    {
      get => _requestedTime;
      set
      {
        _requestedTime = value;
        OnPropertyChanged();
      }
    }

    private string _customerName;
    public string CustomerName
    {
      get => _customerName;
      set
      {
        _customerName = value;
        OnPropertyChanged();
      }
    }

    private string _customerContact;
    public string CustomerContact
    {
      get => _customerContact;
      set
      {
        _customerContact = value;
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

    public ICommand SubmitPickupRequestCommand { get; }

    private async Task SubmitPickupRequest()
    {
      try
      {
        var request = new PickupRequest
        {
          PickupLocation = PickupLocation,
          Destination = Destination,
          RequestedTime = RequestedTime,
          CustomerName = CustomerName,
          CustomerContact = CustomerContact,
          Status = Status
        };

        bool isSuccess = await _pickupRequestService.RequestPickup(request);

        if (isSuccess)
        {
          ErrorMessage = "Pickup request submitted successfully!";
          ClearForm();
        }
        else
        {
          ErrorMessage = "Failed to submit pickup request.";
        }
      }
      catch (Exception ex)
      {
        ErrorMessage = $"An error occurred: {ex.Message}";
      }
    }

    private void ClearForm()
    {
      PickupLocation = string.Empty;
      Destination = string.Empty;
      RequestedTime = DateTime.Now;
      CustomerName = string.Empty;
      CustomerContact = string.Empty;
      Status = string.Empty;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
