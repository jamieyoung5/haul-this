using HaulThis.Services;

namespace HaulThis.Test.Integration;

public class ManageTripsTest : DisposableIntegrationTest
{
    private readonly ITripService _tripService;
    
    public ManageTripsTest()
    {
        _tripService = new TripService(_databaseService);
    }

    [Fact]
    public async Task MarkAsDelivered_ShouldMarkTripItemAsDelivered()
    {
        // Arrange
        var itemId = 1;
        var currentDate = DateTime.UtcNow;

        // Simulate inserting a vehicle, user, bill, trip, and item into the database
        _databaseService.Execute("INSERT INTO vehicle (vehicleName) VALUES (@p0)", "Truck A");
        _databaseService.Execute("INSERT INTO users (roleId, firstName, lastName, email, phoneNumber, address, createdAt) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)",
            3, "John", "Doe", "john.doe@example.com", "1234567890", "123 Main St", currentDate);
    
        var vehicleId = _databaseService.QueryRow("SELECT uniqueId FROM vehicle WHERE vehicleName = @p0", "Truck A").GetInt32(0);
        var userId = _databaseService.QueryRow("SELECT Id FROM users WHERE email = @p0", "john.doe@example.com").GetInt32(0);

        _databaseService.Execute("INSERT INTO bill (userId) VALUES (@p0)", userId);
        var billId = _databaseService.QueryRow("SELECT Id FROM bill WHERE userId = @p0", userId).GetInt32(0);

        _databaseService.Execute("INSERT INTO trip (vehicleId, driverId, date) VALUES (@p0, @p1, @p2)", vehicleId, userId, currentDate);
        var tripId = _databaseService.QueryRow("SELECT Id FROM trip WHERE vehicleId = @p0 AND driverId = @p1", vehicleId, userId).GetInt32(0);

        _databaseService.Execute("INSERT INTO item (tripId, billId, itemWeight, delivered) VALUES (@p0, @p1, @p2, @p3)", tripId, billId, 100, 0);

        // Act
        int result = await _tripService.MarkItemAsDelivered(tripId, itemId);

        // Assert
        Assert.Equal(1, result);

        using (var reader = _databaseService.Query("SELECT delivered FROM item WHERE Id = @p0", itemId))
        {
            reader.Read();
            bool isDelivered = reader.GetBoolean(0);
            Assert.True(isDelivered);
        }
    }

    [Fact]
public async Task GetTripByDateAsync_ShouldReturnTrips_WhenTripsExistForDate()
{
    // Arrange
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

    _databaseService.Execute("INSERT INTO waypoint (tripId, userId, location, estimatedTime) VALUES (@p0, @p1, @p2, @p3)", tripId, driverId, "Location A", date.AddHours(2));

    // Act
    var trips = await _tripService.GetTripByDateAsync(date.Date);

    // Assert
    Assert.NotNull(trips);
    var tripList = trips.ToList();
    Assert.Single(tripList);
    Assert.Equal(tripId, tripList[0].Id);
    Assert.Equal("Truck A", tripList[0].Vehicle.VehicleName);
    Assert.Equal("John Doe", tripList[0].Driver.FirstName);
    Assert.Single(tripList[0].TripManifest);
    Assert.Equal(100, tripList[0].TripManifest[0].ItemWeight);
    Assert.Equal(
        expected: date.AddHours(2).ToString("yyyy-MM-dd HH:mm:ss"),
        actual: tripList[0].TripManifest[0].Waypoint.EstimatedTime.ToString("yyyy-MM-dd HH:mm:ss")
    );

}



}