using HaulThis.Services;
using HaulThis.ViewModels;
using Moq;
using Xunit;

namespace HaulThis.Tests.ViewModels
{
    public class TrackingViewModelTests
    {
        private readonly TrackingViewModel _viewModel;
        private readonly Mock<TrackingService> _mockTrackingService;

        public TrackingViewModelTests()
        {
            _mockTrackingService = new Mock<TrackingService>(null); // Mock TrackingService

            _viewModel = new TrackingViewModel(_mockTrackingService.Object);
        }

        [Fact]
        public async Task TrackItemCommand_UpdatesViewModel_ForValidTrackingId()
        {
            // Arrange
            _mockTrackingService.Setup(service => service.GetTrackingInfo("1"))
                .ReturnsAsync(new TrackingInfo { CurrentLocation = "Location A", ETA = DateTime.UtcNow.AddHours(1), Status = "In Transit" });

            _viewModel.TrackingId = "1";

            // Act
            _viewModel.TrackItemCommand.Execute(null); // Use Execute instead of ExecuteAsync

            // Assert
            Assert.Equal("Location A", _viewModel.CurrentLocation);
            Assert.Equal("In Transit", _viewModel.Status);
            Assert.False(string.IsNullOrEmpty(_viewModel.ETA.ToString()));
        }

        [Fact]
        public async Task TrackItemCommand_SetsError_ForInvalidTrackingId()
        {
            // Arrange
            _mockTrackingService.Setup(service => service.GetTrackingInfo("9999"))
                .ReturnsAsync((TrackingInfo)null);

            _viewModel.TrackingId = "9999";

            // Act
            _viewModel.TrackItemCommand.Execute(null); // Use Execute instead of ExecuteAsync

            // Assert
            Assert.Equal("Invalid tracking ID. Please try again.", _viewModel.ErrorMessage);
        }

        [Fact]
        public async Task TrackItemCommand_HandlesException()
        {
            // Arrange
            _mockTrackingService.Setup(service => service.GetTrackingInfo(It.IsAny<string>()))
                .Throws(new Exception("Something went wrong"));

            _viewModel.TrackingId = "1";

            // Act
            _viewModel.TrackItemCommand.Execute(null); // Use Execute instead of ExecuteAsync

            // Assert
            Assert.Contains("An error occurred: Something went wrong", _viewModel.ErrorMessage);
        }
    }
}
