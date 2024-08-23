using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using HaulThis.Models;
using HaulThis.Services;
using Microsoft.Extensions.Logging;

namespace HaulThis.Tests
{
  public class PickupRequestServiceTests
  {
    // Test successful pickup request
    [Fact]
    public async Task RequestPickup_SuccessfulInsertion_ReturnsTrue()
    {
      var mockDatabaseService = new Mock<IDatabaseService>();
      mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>()))
        .Returns(1); 

      var mockLogger = new Mock<ILogger<PickupRequestService>>();
      var service = new PickupRequestService(mockDatabaseService.Object);

      var pickupRequest = new PickupRequest
      {
        PickupLocation = "Location",
        Destination = "Destination",
        RequestedTime = DateTime.Now,
        CustomerName = "Customer",
        CustomerContact = "Contact",
        Status = "Status"
      };

      var result = await service.RequestPickup(pickupRequest);

      Assert.True(result);
      mockDatabaseService.Verify(db => db.Execute(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
    }

    // Test unsuccessful pickup request
    [Fact]
    public async Task RequestPickup_UnsuccessfulInsertion_ReturnsFalse()
    {
      // Arrange
      var mockDatabaseService = new Mock<IDatabaseService>();
      mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>()))
                         .Returns(0); // Simulate unsuccessful insertion (0 rows affected)

      var mockLogger = new Mock<ILogger<PickupRequestService>>();
      var service = new PickupRequestService(mockDatabaseService.Object);

      var pickupRequest = new PickupRequest
      {
        PickupLocation = "Location",
        Destination = "Destination",
        RequestedTime = DateTime.Now,
        CustomerName = "Customer",
        CustomerContact = "Contact",
        Status = "Status"
      };

      var result = await service.RequestPickup(pickupRequest);

      Assert.False(result);
      mockDatabaseService.Verify(db => db.Execute(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
    }

    // Test exception handling in pickup request
    [Fact]
    public async Task RequestPickup_ExceptionThrown_ReturnsFalse()
    {
      var mockDatabaseService = new Mock<IDatabaseService>();
      mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>()))
                         .Throws(new Exception("Database error")); // Simulate an exception

      var mockLogger = new Mock<ILogger<PickupRequestService>>();
      var service = new PickupRequestService(mockDatabaseService.Object);

      var pickupRequest = new PickupRequest
      {
        PickupLocation = "Location",
        Destination = "Destination",
        RequestedTime = DateTime.Now,
        CustomerName = "Customer",
        CustomerContact = "Contact",
        Status = "Status"
      };

      var result = await service.RequestPickup(pickupRequest);

      Assert.False(result);
      mockDatabaseService.Verify(db => db.Execute(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
    }
  }
}
