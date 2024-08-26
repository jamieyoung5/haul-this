using Xunit;
using Moq;
using HaulThis.ViewModels;
using HaulThis.Services;
using System.Threading.Tasks;

namespace HaulThis.Test.ViewModels;

public class TrackingViewModelTests
{
    private readonly Mock<ITrackingService> _trackingServiceMock;
    private readonly TrackingViewModel _viewModel;

    public TrackingViewModelTests()
    {
        _trackingServiceMock = new Mock<ITrackingService>();
        _viewModel = new TrackingViewModel(_trackingServiceMock.Object);
    }

    [Fact]
    public async Task TrackItem_ValidTrackingId_ShouldUpdateTrackingData()
    {
        // Arrange
        var trackingInfo = new TrackingInfo
        {
            CurrentLocation = "New York",
            ETA = DateTime.UtcNow.AddHours(5),
            Status = "In Transit"
        };

        _trackingServiceMock
            .Setup(service => service.GetTrackingInfo(It.IsAny<int>()))
            .ReturnsAsync(trackingInfo);

        _viewModel.TrackingId = 1;

        // Act
        _viewModel.TrackItemCommand.Execute(null);

        // Assert
        Assert.Equal("New York", _viewModel.CurrentLocation);
        Assert.Equal("In Transit", _viewModel.Status);
        Assert.True(_viewModel.ETA.HasValue);
        Assert.Empty(_viewModel.ErrorMessage);
    }

    [Fact]
    public async Task TrackItem_InvalidTrackingId_ShouldShowErrorMessage()
    {
        // Arrange
        _trackingServiceMock
            .Setup(service => service.GetTrackingInfo(It.IsAny<int>()))
            .ReturnsAsync((TrackingInfo)null);

        _viewModel.TrackingId = -1;

        // Act
        _viewModel.TrackItemCommand.Execute(null);

        // Assert
        Assert.Empty(_viewModel.CurrentLocation);
        Assert.Null(_viewModel.ETA);
        Assert.Empty(_viewModel.Status);
        Assert.Equal("Invalid tracking ID. Please try again.", _viewModel.ErrorMessage);
    }

    [Fact]
    public async Task TrackItem_ServiceThrowsException_ShouldShowErrorMessage()
    {
        // Arrange
        _trackingServiceMock
            .Setup(service => service.GetTrackingInfo(It.IsAny<int>()))
            .ThrowsAsync(new System.Exception("Service error"));

        _viewModel.TrackingId = 1;

        // Act
        _viewModel.TrackItemCommand.Execute(null);

        // Assert
        Assert.Empty(_viewModel.CurrentLocation);
        Assert.Null(_viewModel.ETA);
        Assert.Empty(_viewModel.Status);
        Assert.Contains("An error occurred", _viewModel.ErrorMessage);
    }
}
