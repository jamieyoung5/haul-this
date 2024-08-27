using HaulThis.Services;

namespace HaulThis.Test.Services;

public class TrackingServiceTests
{
    private readonly Mock<IDatabaseService> _databaseServiceMock;
    private readonly TrackingService _trackingService;

    public TrackingServiceTests()
    {
        _databaseServiceMock = new Mock<IDatabaseService>();
        _trackingService = new TrackingService(_databaseServiceMock.Object);
    }

    [Fact]
    public async Task GetTrackingInfo_ValidTrackingId_ShouldReturnTrackingInfo()
    {
        // Arrange
        int trackingId = 1;
        string expectedLocation = "Gondor";
        var expectedArrivalTime = DateTime.UtcNow.AddHours(-1);

        Mock<IDataReader> readerMock = new Mock<IDataReader>();
        readerMock.SetupSequence(reader => reader.Read())
            .Returns(true)
            .Returns(false);
        readerMock.Setup(reader => reader.GetString(0)).Returns(expectedLocation);
        readerMock.Setup(reader => reader.GetDateTime(1)).Returns(expectedArrivalTime);

        _databaseServiceMock.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(readerMock.Object);

        // Act
        var trackingInfo = await _trackingService.GetTrackingInfo(trackingId);

        // Assert
        Assert.NotNull(trackingInfo);
        Assert.Equal(expectedLocation, trackingInfo.CurrentLocation);
        Assert.Equal("In Transit", trackingInfo.Status);
    }

    [Fact]
    public async Task GetTrackingInfo_InvalidTrackingId_ShouldReturnNull()
    {
        // Arrange
        _databaseServiceMock.Setup(db => db.CreateConnection()).Returns(true);
        Mock<IDataReader> readerMock = new Mock<IDataReader>();
        readerMock.Setup(reader => reader.Read()).Returns(false);
        _databaseServiceMock.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(readerMock.Object);

        // Act
        var trackingInfo = await _trackingService.GetTrackingInfo(-1);

        // Assert
        Assert.Null(trackingInfo);
    }

    [Fact]
    public async Task GetTrackingInfo_DatabaseConnectionFails_ShouldReturnNull()
    {
        // Arrange
        _databaseServiceMock.Setup(db => db.CreateConnection()).Returns(false);

        // Act
        var trackingInfo = await _trackingService.GetTrackingInfo(1);

        // Assert
        Assert.Null(trackingInfo);
    }
}