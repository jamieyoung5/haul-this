using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;
using System.Windows.Input;

namespace HaulThis.Tests
{
  public class PickupRequestModelTests
  {
    [Fact]
    public void PropertyChanged_Event_Fires_When_Property_Changes()
    {
      var mockService = new Mock<IPickupRequestService>();
      var viewModel = new PickupRequestModel(mockService.Object);
      bool eventFired = false;

      viewModel.PropertyChanged += (sender, args) =>
      {
        if (args.PropertyName == nameof(PickupRequestModel.PickupLocation))
        {
          eventFired = true;
        }
      };

      viewModel.PickupLocation = "New Location";

      Assert.True(eventFired);
    }

    // Test Command Execution for Successful Request
    [Fact]
    public async Task SubmitPickupRequestCommand_Successful_Request_Sets_ErrorMessage_Correctly()
    {
      var mockService = new Mock<IPickupRequestService>();
      mockService.Setup(service => service.RequestPickup(It.IsAny<PickupRequest>()))
                 .ReturnsAsync(true);

      var viewModel = new PickupRequestModel(mockService.Object)
      {
        PickupLocation = "Location",
        Destination = "Destination",
        RequestedTime = DateTime.Now,
        CustomerName = "Customer",
        CustomerContact = "Contact",
        Status = "Status"
      };

      ((ICommand)viewModel.SubmitPickupRequestCommand).Execute(null);
      await Task.Delay(100);

      // Assert
      Assert.Equal("Pickup request submitted successfully!", viewModel.ErrorMessage);
    }

    // Test Command Execution for failed pickup request
    [Fact]
    public async Task SubmitPickupRequestCommand_Failed_Request_Sets_ErrorMessage_Correctly()
    {
      var mockService = new Mock<IPickupRequestService>();
      mockService.Setup(service => service.RequestPickup(It.IsAny<PickupRequest>()))
           .ReturnsAsync(false);

      var viewModel = new PickupRequestModel(mockService.Object)
      {
        PickupLocation = "Location",
        Destination = "Destination",
        RequestedTime = DateTime.Now,
        CustomerName = "Customer",
        CustomerContact = "Contact",
        Status = "Status"
      };

      ((ICommand)viewModel.SubmitPickupRequestCommand).Execute(null);
      await Task.Delay(100);

      Assert.Equal("Failed to submit pickup request.", viewModel.ErrorMessage);
    }

    // Test Command Execution for Exception Handling
    [Fact]
    public async Task SubmitPickupRequestCommand_Exception_Handled_Correctly()
    {
      var mockService = new Mock<IPickupRequestService>();
      mockService.Setup(service => service.RequestPickup(It.IsAny<PickupRequest>()))
                 .ThrowsAsync(new Exception("Test exception"));

      var viewModel = new PickupRequestModel(mockService.Object)
      {
        PickupLocation = "Location",
        Destination = "Destination",
        RequestedTime = DateTime.Now,
        CustomerName = "Customer",
        CustomerContact = "Contact",
        Status = "Status"
      };

      ((ICommand)viewModel.SubmitPickupRequestCommand).Execute(null);
      await Task.Delay(100);

      Assert.StartsWith("An error occurred: Test exception", viewModel.ErrorMessage);
    }
  }
}
