using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HaulThis.Models;
using HaulThis.Services;
using Microsoft.Maui.Controls;

namespace HaulThis.ViewModels
{
  public class PickupRequestModel : INotifyPropertyChanged
  {
    private readonly IPickupRequestService _pickupRequestService;
    private int _requestId;
    private PickupDeliveryRequest _pickupRequest;
    private bool _isRequestInfoVisible;
    private bool _isErrorVisible;
    private string _errorMessage;

    public PickupRequestModel(IPickupRequestService pickupRequestService)
    {
      _pickupRequestService = pickupRequestService ?? throw new ArgumentNullException(nameof(pickupRequestService));
      GetPickupRequestInfoCommand = new Command(async () => await GetPickupRequestInfo());
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

    public ICommand GetPickupRequestInfoCommand { get; }

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
