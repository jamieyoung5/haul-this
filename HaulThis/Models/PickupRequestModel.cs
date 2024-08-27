using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HaulThis.Models;
using HaulThis.Services;
using Microsoft.Maui.Controls;

namespace HaulThis.Models
{
  public class PickupRequestModel : INotifyPropertyChanged
  {
    private readonly IPickupRequestService _pickupRequestService;
    private int _requestId;
    private PickupDeliveryRequest _pickupRequest;
    private bool _isRequestInfoVisible;
    private bool _isErrorVisible;
    private string _errorMessage;
    private int _createdRequestId; // New property to store the created request ID

    public PickupRequestModel(IPickupRequestService pickupRequestService)
    {
      _pickupRequestService = pickupRequestService ?? throw new ArgumentNullException(nameof(pickupRequestService));
      GetPickupRequestInfoCommand = new Command(async () => await GetPickupRequestInfo());
      CreatePickupRequestCommand = new Command(async () => await CreatePickupRequest()); // Add command for creating request
    }

    public int RequestId
    {
      get => _requestId;
      set
      {
        _requestId = value;
        OnPropertyChanged();
      }
    }

    public PickupDeliveryRequest PickupDeliveryRequest
    {
      get => _pickupRequest;
      set
      {
        _pickupRequest = value;
        OnPropertyChanged();
      }
    }

    public bool IsRequestInfoVisible
    {
      get => _isRequestInfoVisible;
      set
      {
        _isRequestInfoVisible = value;
        OnPropertyChanged();
      }
    }

    public bool IsErrorVisible
    {
      get => _isErrorVisible;
      set
      {
        _isErrorVisible = value;
        OnPropertyChanged();
      }
    }

    public string ErrorMessage
    {
      get => _errorMessage;
      set
      {
        _errorMessage = value;
        OnPropertyChanged();
      }
    }

    public int CreatedRequestId // Property for displaying created request ID
    {
      get => _createdRequestId;
      set
      {
        _createdRequestId = value;
        OnPropertyChanged();
      }
    }

    public ICommand GetPickupRequestInfoCommand { get; }
    public ICommand CreatePickupRequestCommand { get; } // Command for creating request

    private async Task CreatePickupRequest()
    {
      IsErrorVisible = false;

      try
      {
        var request = new PickupDeliveryRequest
        {
          CustomerId = 1, // Set appropriate value
          PickupLocation = "Sample Pickup Location",
          DeliveryLocation = "Sample Delivery Location",
          RequestedPickupDate = DateTime.UtcNow,
          RequestedDeliveryDate = DateTime.UtcNow.AddDays(1),
          Status = "Pending"
        };

        // Call the service to create the pickup request
        int requestId = await _pickupRequestService.CreatePickupRequest(request);

        if (requestId > 0) // Assuming the result indicates success
        {
          CreatedRequestId = requestId; // Set the created request ID
          ErrorMessage = $"Pickup request created successfully. Your Request ID is {requestId}.";
        }
        else
        {
          ErrorMessage = "Failed to create pickup request.";
          IsErrorVisible = true;
        }
      }
      catch (Exception ex)
      {
        ErrorMessage = $"An error occurred: {ex.Message}";
        IsErrorVisible = true;
      }
    }

    private async Task GetPickupRequestInfo()
    {
      IsErrorVisible = false;
      IsRequestInfoVisible = false;

      try
      {
        var request = await _pickupRequestService.GetPickupRequestInfo(RequestId);

        if (request != null)
        {
          PickupDeliveryRequest = request;
          IsRequestInfoVisible = true;
        }
        else
        {
          ErrorMessage = "Pickup request not found.";
          IsErrorVisible = true;
        }
      }
      catch (Exception ex)
      {
        ErrorMessage = $"An error occurred: {ex.Message}";
        IsErrorVisible = true;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
