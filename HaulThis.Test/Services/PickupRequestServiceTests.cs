using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.Test.Services;

public class PickupRequestServiceTests
{
  private readonly Mock<IDatabaseService> _mockDatabaseService;
  private readonly Mock<ILoggerFactory> _mockLoggerFactory;
  private readonly Mock<ILogger<PickupRequestService>> _mockLogger;
  private readonly PickupRequestService _service;

  public PickupRequestServiceTests()
  {
    _mockDatabaseService = new Mock<IDatabaseService>();
    _mockLoggerFactory = new Mock<ILoggerFactory>();
    _mockLogger = new Mock<ILogger<PickupRequestService>>();

    _mockLoggerFactory
        .Setup(factory => factory.CreateLogger<PickupRequestService>())
        .Returns(_mockLogger.Object);

    _service = new PickupRequestService(_mockDatabaseService.Object, _mockLoggerFactory.Object);
  }

  [Fact]
  public async Task GetPickupRequestInfo_ReturnsPickupDeliveryRequest_WhenDataExists()
  {
    // Arrange
    var requestId = 1;
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

    var mockReader = new Mock<IDataReader>();
    mockReader.SetupSequence(reader => reader.Read())
        .Returns(true)  // Simulate that there is data
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
    _mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(2));
  }

  [Fact]
  public async Task GetPickupRequestInfo_ReturnsNull_WhenNoDataExists()
  {
    // Arrange
    var requestId = 1;
    var mockReader = new Mock<IDataReader>();
    mockReader.Setup(reader => reader.Read()).Returns(false); // Simulate no data

    _mockDatabaseService
        .Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
        .Returns(mockReader.Object);

    // Act
    var result = await _service.GetPickupRequestInfo(requestId);

    // Assert
    Assert.Null(result);
    _mockLogger.Verify(logger => logger.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
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
    var expectedRequestId = 123;

    _mockDatabaseService
        .Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
        .Returns(expectedRequestId);

    // Act
    var result = await _service.CreatePickupRequest(request);

    // Assert
    Assert.Equal(expectedRequestId, result);
    _mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(2));
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

    // Act & Assert
    await Assert.ThrowsAsync<Exception>(() => _service.CreatePickupRequest(request));
    _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
  }
}
