using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Services;

namespace HaulThis.Test.Repositories;

public class TripRepositoryTests
{
    private readonly Mock<IDatabaseService> _mockDatabaseService;
    private readonly TripRepository _subject;

    public TripRepositoryTests()
    {
        _mockDatabaseService = new Mock<IDatabaseService>();
        _subject = new TripRepository(_mockDatabaseService.Object);
    }

    [Fact]
    public async Task GetTripByDateAsync_WhenTripsExist_ShouldReturnTrips()
    {
        // Arrange
        Mock<IDataReader> mockReader = new();
        List<Trip> trips = new List<Trip>
        {
            new()
            {
                Id = 1,
                Vehicle = new Vehicle { LicensePlate = "Truck A" },
                Driver = new User { FirstName = "John Doe" },
                TripManifest = new List<Delivery>
                {
                    new()
                    {
                        Id = 1,
                        ItemWeight = 100,
                        CustomerName = "Jane Smith",
                        CustomerPhone = "1234567890",
                        Waypoint = new Waypoint
                        {
                            Location = "Location A",
                            EstimatedTime = DateTime.UtcNow.AddHours(2)
                        }
                    }
                }
            }
        };

        SetupMockReader(mockReader, trips);

        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns(mockReader.Object);

        // Act
        IEnumerable<Trip> result = await _subject.GetTripByDateAsync(DateTime.UtcNow);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var trip = result.First();
        Assert.Equal(1, trip.Id);
        Assert.Equal("Truck A", trip.Vehicle.LicensePlate);
        Assert.Equal("John Doe", trip.Driver.FirstName);

        Assert.NotNull(trip.TripManifest);
        Assert.Single(trip.TripManifest);

        var delivery = trip.TripManifest.First();
        Assert.Equal(1, delivery.Id);
        Assert.Equal(100, delivery.ItemWeight);
        Assert.Equal("Jane Smith", delivery.CustomerName);
        Assert.Equal("1234567890", delivery.CustomerPhone);
        Assert.NotNull(delivery.Waypoint);
        Assert.Equal("Location A", delivery.Waypoint.Location);
        Assert.Equal(DateTime.UtcNow.AddHours(2).Hour, delivery.Waypoint.EstimatedTime.Hour);
    }

    [Fact]
    public async Task GetTripByDateAsync_WhenNoTripsExist_ShouldReturnEmptyList()
    {
        // Arrange
        Mock<IDataReader> mockReader = new();
        mockReader.Setup(r => r.Read()).Returns(false);
        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>(), It.IsAny<object[]>())).Returns(mockReader.Object);

        // Act
        IEnumerable<Trip> result = await _subject.GetTripByDateAsync(DateTime.UtcNow);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTripByDateAsync_WhenTripsWithMultipleManifestsExist_ShouldReturnTripsWithManifests()
    {
        // Arrange
        Mock<IDataReader> mockReader = new();
        var trip = new Trip
        {
            Id = 1,
            Vehicle = new Vehicle { LicensePlate = "Truck A" },
            Driver = new User { FirstName = "John Doe" },
            TripManifest = new List<Delivery>
            {
                new()
                {
                    Id = 1,
                    ItemWeight = 100,
                    CustomerName = "Jane Smith",
                    CustomerPhone = "1234567890",
                    Waypoint = new Waypoint
                    {
                        Location = "Location A",
                        EstimatedTime = DateTime.UtcNow.AddHours(2)
                    }
                },
                new()
                {
                    Id = 2,
                    ItemWeight = 150,
                    CustomerName = "Jim Brown",
                    CustomerPhone = "0987654321",
                    Waypoint = new Waypoint
                    {
                        Location = "Location B",
                        EstimatedTime = DateTime.UtcNow.AddHours(3)
                    }
                }
            }
        };

        SetupMockReader(mockReader, new List<Trip> { trip });

        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>(), It.IsAny<object[]>())).Returns(mockReader.Object);

        // Act
        IEnumerable<Trip> result = await _subject.GetTripByDateAsync(DateTime.UtcNow);

        // Assert
        Assert.NotNull(result);
        List<Trip> resultList = result.ToList();
        Assert.Single(resultList);

        Assert.Equal(2, resultList[0].TripManifest.Count);
        Assert.Equal("Jane Smith", resultList[0].TripManifest[0].CustomerName);
        Assert.Equal("Jim Brown", resultList[0].TripManifest[1].CustomerName);
    }

    private static void SetupMockReader(Mock<IDataReader> mockReader, List<Trip> trips)
    {
        int currentIndex = -1;
        var deliveries = trips.SelectMany(t => t.TripManifest.Select(m => new { Trip = t, Manifest = m })).ToList();

        mockReader.Setup(r => r.Read()).Returns(() =>
        {
            currentIndex++;
            return currentIndex < deliveries.Count;
        });

        mockReader.Setup(r => r.GetInt32(0)).Returns(() => deliveries[currentIndex].Trip.Id);
        mockReader.Setup(r => r.GetString(1)).Returns(() => deliveries[currentIndex].Trip.Vehicle.LicensePlate);
        mockReader.Setup(r => r.GetString(2)).Returns(() => deliveries[currentIndex].Trip.Driver.FirstName);
        mockReader.Setup(r => r.GetString(3)).Returns(() => deliveries[currentIndex].Manifest.Waypoint.Location);
        mockReader.Setup(r => r.GetDateTime(4)).Returns(() => deliveries[currentIndex].Manifest.Waypoint.EstimatedTime);
        mockReader.Setup(r => r.GetInt32(5)).Returns(() => deliveries[currentIndex].Manifest.Id);
        mockReader.Setup(r => r.GetDecimal(6)).Returns(() => deliveries[currentIndex].Manifest.ItemWeight);
        mockReader.Setup(r => r.GetString(7)).Returns(() => deliveries[currentIndex].Manifest.CustomerName);
        mockReader.Setup(r => r.GetString(8)).Returns(() => deliveries[currentIndex].Manifest.CustomerPhone);
    }
}