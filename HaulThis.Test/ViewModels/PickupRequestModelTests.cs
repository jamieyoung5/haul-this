using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.Test.ViewModels;

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
  public void Constructor_ThrowsArgumentNullException_WhenServiceIsNull()
  {
    Assert.Throws<ArgumentNullException>(() => new PickupRequestModel(null));
  }

  [Fact]
  public async Task CreatePickupRequestCommand_SuccessfullySetsCreatedRequestId()
  {
    // Arrange
    var request = new PickupDeliveryRequest
    {
      CustomerId = 1,
      PickupLocation = "Sample Pickup Location",
      DeliveryLocation = "Sample Delivery Location",
      RequestedPickupDate = DateTime.UtcNow,
      RequestedDeliveryDate = DateTime.UtcNow.AddDays(1),
      Status = "Pending"
    };
    int expectedRequestId = 123;
    _mockPickupRequestService
        .Setup(service => service.CreatePickupRequest(It.IsAny<PickupDeliveryRequest>()))
        .ReturnsAsync(expectedRequestId);

    // Act
    _viewModel.CreatePickupRequestCommand.Execute(null);

    // Assert
    Assert.Equal(expectedRequestId, _viewModel.CreatedRequestId);
    Assert.Equal($"Pickup request created successfully. Your Request ID is {expectedRequestId}.", _viewModel.ErrorMessage);
    Assert.False(_viewModel.IsErrorVisible);
  }

  [Fact]
  public async Task CreatePickupRequestCommand_Fails_WhenServiceReturnsZero()
  {
    // Arrange
    _mockPickupRequestService
        .Setup(service => service.CreatePickupRequest(It.IsAny<PickupDeliveryRequest>()))
        .ReturnsAsync(0);

    // Act
    _viewModel.CreatePickupRequestCommand.Execute(null);

    // Assert
    Assert.Equal("Failed to create pickup request.", _viewModel.ErrorMessage);
    Assert.True(_viewModel.IsErrorVisible);
  }

  [Fact]
  public async Task GetPickupRequestInfoCommand_SuccessfullySetsPickupRequest()
  {
    // Arrange
    var request = new PickupDeliveryRequest
    {
      Id = 1,
      CustomerId = 1,
      PickupLocation = "Sample Pickup Location",
      DeliveryLocation = "Sample Delivery Location",
      RequestedPickupDate = DateTime.UtcNow,
      RequestedDeliveryDate = DateTime.UtcNow.AddDays(1),
      Status = "Pending"
    };
    _mockPickupRequestService
        .Setup(service => service.GetPickupRequestInfo(It.IsAny<int>()))
        .ReturnsAsync(request);

    _viewModel.RequestId = 1;

    // Act
    _viewModel.GetPickupRequestInfoCommand.Execute(null);

    // Assert
    Assert.Equal(request, _viewModel.PickupDeliveryRequest);
    Assert.True(_viewModel.IsRequestInfoVisible);
    Assert.False(_viewModel.IsErrorVisible);
  }

  [Fact]
  public async Task GetPickupRequestInfoCommand_Fails_WhenRequestIsNotFound()
  {
    // Arrange
    _mockPickupRequestService
        .Setup(service => service.GetPickupRequestInfo(It.IsAny<int>()))
        .ReturnsAsync((PickupDeliveryRequest)null);

    _viewModel.RequestId = 1;

    // Act
    _viewModel.GetPickupRequestInfoCommand.Execute(null);

    // Assert
    Assert.Equal("Pickup request not found.", _viewModel.ErrorMessage);
    Assert.True(_viewModel.IsErrorVisible);
    Assert.False(_viewModel.IsRequestInfoVisible);
  }

  [Fact]
  public async Task GetPickupRequestInfoCommand_HandlesException()
  {
    // Arrange
    _mockPickupRequestService
        .Setup(service => service.GetPickupRequestInfo(It.IsAny<int>()))
        .ThrowsAsync(new Exception("Test Exception"));

    _viewModel.RequestId = 1;

    // Act
    _viewModel.GetPickupRequestInfoCommand.Execute(null);

    // Assert
    Assert.Equal("An error occurred: Test Exception", _viewModel.ErrorMessage);
    Assert.True(_viewModel.IsErrorVisible);
    Assert.False(_viewModel.IsRequestInfoVisible);
  }
}
