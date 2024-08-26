using Xunit;
using HaulThis.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using HaulThis.Test.Integration;
using Microsoft.Data.SqlClient;

namespace HaulThis.Test.Integration;

public class TrackItemTest : DisposableIntegrationTest
{
    private readonly ITrackingService _trackingService;
    
    public TrackItemTest()
    {
        _trackingService = new TrackingService(_databaseService);
    }
    
    [Fact]
    public async Task GetTrackingInfo_ValidTrackingId_ShouldReturnTrackingInfo()
    {
        var date = DateTime.UtcNow;

        // Insert necessary data into the database
        _databaseService.Execute("INSERT INTO vehicle (vehicleName) VALUES (@p0)", "Truck A");
        var vehicleId = _databaseService.QueryRow("SELECT uniqueId FROM vehicle WHERE vehicleName = @p0", "Truck A").GetInt32(0);

        _databaseService.Execute("INSERT INTO users (roleId, firstName, lastName, email, phoneNumber, address, createdAt) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)",
            3, "John", "Doe", "john.doe@example.com", "1234567890", "123 Main St", date);
        var driverId = _databaseService.QueryRow("SELECT Id FROM users WHERE email = @p0", "john.doe@example.com").GetInt32(0);

        _databaseService.Execute("INSERT INTO bill (userId) VALUES (@p0)", driverId);
        var billId = _databaseService.QueryRow("SELECT Id FROM bill WHERE userId = @p0", driverId).GetInt32(0);

        // Insert trip with the current date
        _databaseService.Execute("INSERT INTO trip (vehicleId, driverId, date) VALUES (@p0, @p1, @p2)", vehicleId, driverId, date);
        var tripId = _databaseService.QueryRow("SELECT Id FROM trip WHERE vehicleId = @p0 AND driverId = @p1", vehicleId, driverId).GetInt32(0);

        _databaseService.Execute("INSERT INTO item (tripId, billId, itemWeight, delivered) VALUES (@p0, @p1, @p2, @p3)", tripId, billId, 100, 0);

        _databaseService.Execute("INSERT INTO waypoint (tripId, userId, location, estimatedTime) VALUES (@p0, @p1, @p2, @p3)", tripId, driverId, "New York", date.AddHours(2));
        
        var trackingId = 1;
        
        var trackingInfo = await _trackingService.GetTrackingInfo(trackingId);
        
        Assert.NotNull(trackingInfo);
        Assert.Equal("New York", trackingInfo.CurrentLocation);
        Assert.Equal("In Transit", trackingInfo.Status);
    }

    [Fact]
    public async Task GetTrackingInfo_InvalidTrackingId_ShouldReturnNull()
    {
        var trackingId = -1;
        
        var trackingInfo = await _trackingService.GetTrackingInfo(trackingId);
        
        Assert.Null(trackingInfo);
    }

}
