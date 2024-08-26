using System;
using Microsoft.Maui.Controls;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.Views.Customer
{
  public partial class RequestPickup : ContentPage
  {
    private readonly IPickupRequestService _pickupRequestService;

    public RequestPickup(IPickupRequestService pickupRequestService)
    {
      InitializeComponent();
      _pickupRequestService = pickupRequestService ?? throw new ArgumentNullException(nameof(pickupRequestService));
      BindingContext = new PickupRequestModel(_pickupRequestService);
    }
  }
}