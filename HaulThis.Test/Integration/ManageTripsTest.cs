using HaulThis.Models;
using HaulThis.Repositories;

namespace HaulThis.Test.Integration;

[Collection("Sequential Tests")]
public class ManageTripsTest : DisposableIntegrationTest
{
    private readonly ITripRepository _tripRepository;
    private readonly IItemRepository _itemRepository;

    public ManageTripsTest()
    {
        _tripRepository = new TripRepository(_databaseService);
        _itemRepository = new ItemRepository(_databaseService);
    }

    [Fact]
    public async Task MarkAsDelivered_ShouldMarkTripItemAsDelivered()
    {
        // Arrange
        int itemId = 1;
        var currentDate = DateTime.UtcNow;

        _databaseService.Execute(
            "INSERT INTO vehicle (make, model, year, licensePlate, status, createdAt) VALUES (@p0, @p1, @p2, @p3, @p4, @p5)",
            "Ford", "F-150", 2020, "Truck A", "Available", currentDate);

        _databaseService.Execute(
            "INSERT INTO users (roleId, firstName, lastName, email, phoneNumber, address, createdAt) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)",
            3, "John", "Doe", "john.doe@example.com", "1234567890", "123 Main St", currentDate);

        int vehicleId = _databaseService.QueryRow("SELECT uniqueId FROM vehicle WHERE licensePlate = @p0", "Truck A")
            .GetInt32(0);
        int userId = _databaseService.QueryRow("SELECT Id FROM users WHERE email = @p0", "john.doe@example.com")
            .GetInt32(0);

        _databaseService.Execute(
            "INSERT INTO bill (userId, amount, billDate, dueDate, status) VALUES (@p0, @p1, @p2, @p3, @p4)",
            userId, 500.00m, currentDate, currentDate.AddDays(30), "UNPAID");
        int billId = _databaseService.QueryRow("SELECT Id FROM bill WHERE userId = @p0", userId).GetInt32(0);

        _databaseService.Execute("INSERT INTO trip (vehicleId, driverId, date) VALUES (@p0, @p1, @p2)", vehicleId,
            userId, currentDate);
        int tripId = _databaseService
            .QueryRow("SELECT Id FROM trip WHERE vehicleId = @p0 AND driverId = @p1", vehicleId, userId).GetInt32(0);

        _databaseService.Execute(
            "INSERT INTO item (tripId, billId, pickedUpBy, deliveredBy, itemWeight, delivered) VALUES (@p0, @p1, @p2, @p3, @p4, @p5)",
            tripId, billId, userId, userId, 100.00m, 0);
        itemId = _databaseService.QueryRow("SELECT Id FROM item WHERE tripId = @p0 AND billId = @p1", tripId, billId)
            .GetInt32(0);

        // Act
        int result = await _itemRepository.MarkAsDeliveredAsync(tripId, itemId);

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
            "INSERT INTO waypoint (tripId, userId, location, estimatedTime) VALUES (@p0, @p1, @p2, @p3)",
            tripId, driverId, "Location A", date.AddHours(2));

        // Act
        IEnumerable<Trip> trips = await _tripRepository.GetTripByDateAsync(date.Date);

        // Assert
        Assert.NotNull(trips);
        List<Trip> tripList = trips.ToList();
        Assert.Single(tripList);
        Assert.Equal(tripId, tripList[0].Id);
        Assert.Equal("Truck A", tripList[0].Vehicle.LicensePlate);
        Assert.Equal("John Doe ", $"{tripList[0].Driver.FirstName} {tripList[0].Driver.LastName}");
        Assert.Single(tripList[0].TripManifest);
        Assert.Equal(100.00m, tripList[0].TripManifest[0].ItemWeight);
        Assert.Equal(
            date.AddHours(2).ToString("yyyy-MM-dd HH:mm:ss"),
            tripList[0].TripManifest[0].Waypoint.EstimatedTime.ToString("yyyy-MM-dd HH:mm:ss")
        );
    }
}