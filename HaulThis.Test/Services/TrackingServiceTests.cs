using Xunit;
using Moq;
using HaulThis.Services;
using HaulThis.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

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
        _databaseServiceMock.Setup(db => db.CreateConnection()).Returns(true);
        _databaseServiceMock.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new Mock<IDataReader>().Object);

        var readerMock = new Mock<IDataReader>();
        readerMock.Setup(reader => reader.Read()).Returns(true);
        readerMock.Setup(reader => reader.GetString(0)).Returns("Gondor");
        readerMock.Setup(reader => reader.GetDateTime(1)).Returns(DateTime.UtcNow);

        _databaseServiceMock.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(readerMock.Object);

        // Act
        var trackingInfo = await _trackingService.GetTrackingInfo(1);

        // Assert
        Assert.NotNull(trackingInfo);
        Assert.Equal("Gondor", trackingInfo.CurrentLocation);
        Assert.Equal("In Transit", trackingInfo.Status);
    }

    [Fact]
    public async Task GetTrackingInfo_InvalidTrackingId_ShouldReturnNull()
    {
        // Arrange
        _databaseServiceMock.Setup(db => db.CreateConnection()).Returns(true);
        var readerMock = new Mock<IDataReader>();
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
