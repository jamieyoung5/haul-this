using System;
using System.Threading.Tasks;
using HaulThis.Models;
using HaulThis.Services;
using Microsoft.Extensions.Logging;

namespace HaulThis.Tests.Services
{
  public class PickupRequestServiceTests
  {
    private readonly Mock<IDatabaseService> _databaseServiceMock;
    private readonly Mock<ILogger<PickupRequestService>> _loggerMock;
    private readonly PickupRequestService _pickupRequestService;

    public PickupRequestServiceTests()
    {
      _databaseServiceMock = new Mock<IDatabaseService>();
      _loggerMock = new Mock<ILogger<PickupRequestService>>();
      _pickupRequestService = new PickupRequestService(_databaseServiceMock.Object);
    }

    [Fact]
    public async Task GetPickupRequestInfo_ReturnsRequest_WhenDataIsFound()
    {
      // Arrange
      var mockDataReader = new Mock<IDataReader>();
      mockDataReader.SetupSequence(r => r.Read())
          .Returns(true)
          .Returns(false);
      mockDataReader.Setup(r => r.GetInt32(0)).Returns(1);
      mockDataReader.Setup(r => r.GetInt32(1)).Returns(123);
      mockDataReader.Setup(r => r.GetString(2)).Returns("123 Main St");
      mockDataReader.Setup(r => r.GetString(3)).Returns("456 Elm St");
      mockDataReader.Setup(r => r.GetDateTime(4)).Returns(DateTime.Now.AddDays(1));
      mockDataReader.Setup(r => r.GetDateTime(5)).Returns(DateTime.Now.AddDays(2));
      mockDataReader.Setup(r => r.GetString(6)).Returns("Pending");

      _databaseServiceMock.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
          .Returns(mockDataReader.Object);

      // Act
      var result = await _pickupRequestService.GetPickupRequestInfo(1);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(1, result.Id);
      Assert.Equal(123, result.CustomerId);
      Assert.Equal("123 Main St", result.PickupLocation);
      Assert.Equal("456 Elm St", result.DeliveryLocation);
      Assert.Equal("Pending", result.Status);
    }

    [Fact]
    public async Task GetPickupRequestInfo_ReturnsNull_WhenNoDataIsFound()
    {
      // Arrange
      var mockDataReader = new Mock<IDataReader>();
      mockDataReader.Setup(r => r.Read()).Returns(false);

      _databaseServiceMock.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
          .Returns(mockDataReader.Object);

      // Act
      var result = await _pickupRequestService.GetPickupRequestInfo(1);

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public async Task GetPickupRequestInfo_LogsError_WhenExceptionIsThrown()
    {
      // Arrange
      _databaseServiceMock.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
          .Throws(new Exception("Database error"));

      // Act
      var result = await _pickupRequestService.GetPickupRequestInfo(1);

      // Assert
      Assert.Null(result);
      _loggerMock.Verify(
          x => x.Log(
              LogLevel.Error,
              It.IsAny<EventId>(),
              It.IsAny<It.IsAnyType>(),
              It.IsAny<Exception>(),
              (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
          Times.Once);
    }
  }
}
