using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Tests
{
  public class PickupRequestModelTests
  {
    private readonly Mock<IPickupRequestService> _mockPickupRequestService;
    private readonly PickupRequestModel _viewModel;

    public PickupRequestModelTests()
    {
      _mockPickupRequestService = new Mock<IPickupRequestService>();
      _viewModel = new PickupRequestModel(_mockPickupRequestService.Object);
    }

    [Fact]
    public async Task GetPickupRequestInfo_ShouldSetPickupRequest_WhenRequestIsFound()
    {
      // Arrange
      var expectedRequest = new PickupDeliveryRequest
      {
        Id = 1,
        PickupLocation = "Location A",
        DeliveryLocation = "Location B",
        RequestedPickupDate = DateTime.Now,
        RequestedDeliveryDate = DateTime.Now.AddDays(1),
        Status = "Pending"
      };
      _mockPickupRequestService
          .Setup(service => service.GetPickupRequestInfo(It.IsAny<int>()))
          .ReturnsAsync(expectedRequest);

      _viewModel.RequestId = 1;

      // Act
      _viewModel.GetPickupRequestInfoCommand.Execute(null);

      // Assert
      Assert.NotNull(_viewModel.PickupDeliveryRequest);
      Assert.Equal(expectedRequest.Id, _viewModel.PickupDeliveryRequest.Id);
      Assert.True(_viewModel.IsRequestInfoVisible);
      Assert.False(_viewModel.IsErrorVisible);
      Assert.Null(_viewModel.ErrorMessage);
    }

    [Fact]
    public async Task GetPickupRequestInfo_ShouldSetError_WhenRequestIsNotFound()
    {
      // Arrange
      _mockPickupRequestService
          .Setup(service => service.GetPickupRequestInfo(It.IsAny<int>()))
          .ReturnsAsync((PickupDeliveryRequest)null);

      _viewModel.RequestId = 1;

      // Act
      _viewModel.GetPickupRequestInfoCommand.Execute(null);

      // Assert
      Assert.Null(_viewModel.PickupDeliveryRequest);
      Assert.True(_viewModel.IsErrorVisible);
      Assert.Equal("Pickup request not found.", _viewModel.ErrorMessage);
    }

    [Fact]
    public async Task GetPickupRequestInfo_ShouldSetError_WhenExceptionOccurs()
    {
      // Arrange
      _mockPickupRequestService
          .Setup(service => service.GetPickupRequestInfo(It.IsAny<int>()))
          .ThrowsAsync(new Exception("Database error"));

      _viewModel.RequestId = 1;

      // Act
      _viewModel.GetPickupRequestInfoCommand.Execute(null);

      // Assert
      Assert.True(_viewModel.IsErrorVisible);
      Assert.StartsWith("An error occurred:", _viewModel.ErrorMessage);
    }
  }
}
