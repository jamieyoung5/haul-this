using HaulThis.Services;

namespace HaulThis.Test.Integration;

[Collection("Sequential Tests")]
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

        _databaseService.Execute(
            "INSERT INTO vehicle (make, model, year, licensePlate, status, createdAt) VALUES (@p0, @p1, @p2, @p3, @p4, @p5)",
            "Ford", "F-150", 2020, "Truck A", "Available", date);
        int vehicleId = _databaseService.QueryRow("SELECT uniqueId FROM vehicle WHERE licensePlate = @p0", "Truck A")
            .GetInt32(0);

        _databaseService.Execute(
            "INSERT INTO users (roleId, firstName, lastName, email, phoneNumber, address, createdAt) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)",
            3, "John", "Doe", "john.doe@example.com", "1234567890", "123 Main St", date);
        int driverId = _databaseService.QueryRow("SELECT Id FROM users WHERE email = @p0", "john.doe@example.com")
            .GetInt32(0);

        _databaseService.Execute(
            "INSERT INTO bill (userId, amount, billDate, dueDate, status) VALUES (@p0, @p1, @p2, @p3, @p4)",
            driverId, 500.00m, date, date.AddDays(30), "UNPAID");
        int billId = _databaseService.QueryRow("SELECT Id FROM bill WHERE userId = @p0", driverId).GetInt32(0);

        _databaseService.Execute("INSERT INTO trip (vehicleId, driverId, date) VALUES (@p0, @p1, @p2)", vehicleId,
            driverId, date);
        int tripId = _databaseService
            .QueryRow("SELECT Id FROM trip WHERE vehicleId = @p0 AND driverId = @p1", vehicleId, driverId).GetInt32(0);

        _databaseService.Execute(
            "INSERT INTO item (tripId, billId, pickedUpBy, deliveredBy, itemWeight, delivered) VALUES (@p0, @p1, @p2, @p3, @p4, @p5)",
            tripId, billId, driverId, driverId, 100.00m, 0);

        _databaseService.Execute(
            "INSERT INTO waypoint (tripId, userId, location, estimatedTime) VALUES (@p0, @p1, @p2, @p3)", tripId,
            driverId, "New York", date.AddHours(2));

        int trackingId = 1;

        var trackingInfo = await _trackingService.GetTrackingInfo(trackingId);

        Assert.NotNull(trackingInfo);
        Assert.Equal("New York", trackingInfo.CurrentLocation);
        Assert.Equal("In Transit", trackingInfo.Status);
    }


    [Fact]
    public async Task GetTrackingInfo_InvalidTrackingId_ShouldReturnNull()
    {
        int trackingId = -1;

        var trackingInfo = await _trackingService.GetTrackingInfo(trackingId);

        Assert.Null(trackingInfo);
    }
}