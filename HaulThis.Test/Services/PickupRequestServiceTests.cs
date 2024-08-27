using HaulThis.Models;
using HaulThis.Services;
using Microsoft.Extensions.Logging;

namespace HaulThis.Test.Services;

public class PickupRequestServiceTests
{
    private readonly Mock<IDatabaseService> _mockDatabaseService;
    private readonly Mock<ILogger<PickupRequestService>> _mockLogger;
    private readonly PickupRequestService _service;

    public PickupRequestServiceTests()
    {
        _mockDatabaseService = new Mock<IDatabaseService>();
        _mockLogger = new Mock<ILogger<PickupRequestService>>();

        _service = new PickupRequestService(_mockDatabaseService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetPickupRequestInfo_ReturnsPickupDeliveryRequest_WhenDataExists()
    {
        // Arrange
        int requestId = 1;
        var expectedRequest = new PickupDeliveryRequest
        {
            Id = requestId,
            CustomerId = 1,
            PickupLocation = "Location A",
            DeliveryLocation = "Location B",
            RequestedPickupDate = DateTime.UtcNow,
            RequestedDeliveryDate = DateTime.UtcNow.AddDays(1),
            Status = "Pending"
        };

        Mock<IDataReader> mockReader = new Mock<IDataReader>();
        mockReader.SetupSequence(reader => reader.Read())
            .Returns(true) // Simulate that there is data
            .Returns(false); // Simulate end of data
        mockReader.Setup(reader => reader.GetInt32(0)).Returns(expectedRequest.Id);
        mockReader.Setup(reader => reader.GetInt32(1)).Returns(expectedRequest.CustomerId);
        mockReader.Setup(reader => reader.GetString(2)).Returns(expectedRequest.PickupLocation);
        mockReader.Setup(reader => reader.GetString(3)).Returns(expectedRequest.DeliveryLocation);
        mockReader.Setup(reader => reader.GetDateTime(4)).Returns(expectedRequest.RequestedPickupDate);
        mockReader.Setup(reader => reader.GetDateTime(5)).Returns(expectedRequest.RequestedDeliveryDate);
        mockReader.Setup(reader => reader.GetString(6)).Returns(expectedRequest.Status);

        _mockDatabaseService
            .Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns(mockReader.Object);

        // Act
        var result = await _service.GetPickupRequestInfo(requestId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedRequest.Id, result.Id);
        Assert.Equal(expectedRequest.PickupLocation, result.PickupLocation);

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Starting GetPickupRequestInfo for ID: {requestId}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Query returned data for Request ID: {requestId}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPickupRequestInfo_ReturnsNull_WhenNoDataExists()
    {
        // Arrange
        int requestId = 1;
        Mock<IDataReader> mockReader = new Mock<IDataReader>();
        mockReader.Setup(reader => reader.Read()).Returns(false); // Simulate no data

        _mockDatabaseService
            .Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns(mockReader.Object);

        // Act
        var result = await _service.GetPickupRequestInfo(requestId);

        // Assert
        Assert.Null(result);

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Starting GetPickupRequestInfo for ID: {requestId}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Query did not return any rows for Request ID: {requestId}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task CreatePickupRequest_ReturnsRequestId_WhenSuccess()
    {
        // Arrange
        var request = new PickupDeliveryRequest
        {
            CustomerId = 1,
            PickupLocation = "Location A",
            DeliveryLocation = "Location B",
            RequestedPickupDate = DateTime.UtcNow,
            RequestedDeliveryDate = DateTime.UtcNow.AddDays(1),
            Status = "Pending"
        };
        int expectedRequestId = 123;

        _mockDatabaseService
            .Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns(expectedRequestId);

        // Act
        int result = await _service.CreatePickupRequest(request);

        // Assert
        Assert.Equal(expectedRequestId, result);

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Starting CreatePickupRequest for CustomerId: {request.CustomerId}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Created pickup request with CustomerId: {request.CustomerId}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task CreatePickupRequest_ThrowsException_WhenDatabaseErrorOccurs()
    {
        // Arrange
        var request = new PickupDeliveryRequest
        {
            CustomerId = 1,
            PickupLocation = "Location A",
            DeliveryLocation = "Location B",
            RequestedPickupDate = DateTime.UtcNow,
            RequestedDeliveryDate = DateTime.UtcNow.AddDays(1),
            Status = "Pending"
        };

        _mockDatabaseService
            .Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
            .Throws<Exception>();

        // Act / Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreatePickupRequest(request));

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Starting CreatePickupRequest for CustomerId: {request.CustomerId}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString()
                        .Contains(
                            $"An error occurred while creating pickup request for CustomerId: {request.CustomerId}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }
}