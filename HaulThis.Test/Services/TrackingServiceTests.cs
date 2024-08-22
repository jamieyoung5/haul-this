using System.Data;
using HaulThis.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HaulThis.Tests.Services
{
    public class TrackingServiceTests
    {
        private readonly TrackingService _trackingService;
        private readonly Mock<IDatabaseService> _mockDatabaseService;
        private readonly Mock<ILogger<TrackingService>> _mockLogger;

        public TrackingServiceTests()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _mockLogger = new Mock<ILogger<TrackingService>>();
            _trackingService = new TrackingService(_mockDatabaseService.Object);
        }

        [Fact]
        public async Task GetTrackingInfo_ReturnsCorrectInfo_ForValidTrackingId()
        {
            
            _mockDatabaseService.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns(Mock.Of<IDataReader>(reader =>
                    reader.Read() == true &&
                    reader.GetString(0) == "Location A" &&
                    reader.GetDateTime(1) == DateTime.UtcNow));

         
            var result = await _trackingService.GetTrackingInfo("1");

         
            Assert.NotNull(result);
            Assert.Equal("Location A", result.CurrentLocation);
        }

        [Fact]
        public async Task GetTrackingInfo_ReturnsNull_ForInvalidTrackingId()
        {
            
            _mockDatabaseService.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns(Mock.Of<IDataReader>(reader => reader.Read() == false)); // Simulate no results

           
            var result = await _trackingService.GetTrackingInfo("9999"); // Invalid ID

            
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTrackingInfo_ReturnsLatestWaypoint_ForMultipleWaypoints()
        {
            
            var now = DateTime.UtcNow;

            var mockDataReader = new Mock<IDataReader>();
            mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false); 

            mockDataReader.Setup(r => r.GetString(0)).Returns("Location B");
            mockDataReader.Setup(r => r.GetDateTime(1)).Returns(now.AddHours(-1));

            _mockDatabaseService.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns(mockDataReader.Object);

            
            var result = await _trackingService.GetTrackingInfo("1");

            
            Assert.NotNull(result);
            Assert.Equal("Location B", result.CurrentLocation);
        }
    }
}
