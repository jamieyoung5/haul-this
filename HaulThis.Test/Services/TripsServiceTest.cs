using HaulThis.Models;
using HaulThis.Services;
using Moq;
using System.Data;
using Xunit;

namespace HaulThis.Test.Services
{
    public class TripServiceTests
    {
        private readonly Mock<IDatabaseService> _mockDatabaseService;
        private readonly TripService _subject;

        public TripServiceTests()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _subject = new TripService(_mockDatabaseService.Object);
        }

        [Fact]
        public async Task GetTripByDateAsync_WhenTripsExist_ShouldReturnTrips()
        {
            // Arrange
            var mockReader = new Mock<IDataReader>();
            var trips = new List<Trip>
            {
                new Trip
                {
                    Id = 1,
                    Vehicle = new Vehicle { VehicleName = "Truck A" },
                    Driver = new User { FirstName = "John Doe" },
                    TripManifest = new List<Delivery>
                    {
                        new Delivery
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
            var result = await _subject.GetTripByDateAsync(DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            
            var trip = result.First();
            Assert.Equal(1, trip.Id);
            Assert.Equal("Truck A", trip.Vehicle.VehicleName);
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
            var mockReader = new Mock<IDataReader>();
            mockReader.Setup(r => r.Read()).Returns(false);
            _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>(), It.IsAny<object[]>())).Returns(mockReader.Object);

            // Act
            var result = await _subject.GetTripByDateAsync(DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTripByDateAsync_WhenTripsWithMultipleManifestsExist_ShouldReturnTripsWithManifests()
        {
            // Arrange
            var mockReader = new Mock<IDataReader>();
            var trip = new Trip
            {
                Id = 1,
                Vehicle = new Vehicle { VehicleName = "Truck A" },
                Driver = new User { FirstName = "John Doe" },
                TripManifest = new List<Delivery>
                {
                    new Delivery
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
                    new Delivery
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
            var result = await _subject.GetTripByDateAsync(DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Single(resultList);
            
            Assert.Equal(2, resultList[0].TripManifest.Count);
            Assert.Equal("Jane Smith", resultList[0].TripManifest[0].CustomerName);
            Assert.Equal("Jim Brown", resultList[0].TripManifest[1].CustomerName);
        }

        [Fact]
        public async Task MarkItemAsDelivered_WhenItemDoesNotExist_ShouldReturnZeroRowsAffected()
        {
            // Arrange
            var tripId = 1;
            var itemId = 999; // Non-existent item
            _mockDatabaseService.Setup(ds => ds.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(0);

            // Act
            var result = await _subject.MarkItemAsDelivered(tripId, itemId);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task MarkItemAsDelivered_WhenMultipleItemsAreDelivered_ShouldReturnNumberOfRowsAffected()
        {
            // Arrange
            var tripId = 1;
            var itemId1 = 1;
            var itemId2 = 2;

            _mockDatabaseService.Setup(ds => ds.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);

            // Act
            var result1 = await _subject.MarkItemAsDelivered(tripId, itemId1);
            var result2 = await _subject.MarkItemAsDelivered(tripId, itemId2);

            // Assert
            Assert.Equal(1, result1);
            Assert.Equal(1, result2);
            Assert.Equal(2, result1 + result2);
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
            mockReader.Setup(r => r.GetString(1)).Returns(() => deliveries[currentIndex].Trip.Vehicle.VehicleName);
            mockReader.Setup(r => r.GetString(2)).Returns(() => deliveries[currentIndex].Trip.Driver.FirstName);
            mockReader.Setup(r => r.GetString(3)).Returns(() => deliveries[currentIndex].Manifest.Waypoint.Location);
            mockReader.Setup(r => r.GetDateTime(4)).Returns(() => deliveries[currentIndex].Manifest.Waypoint.EstimatedTime);
            mockReader.Setup(r => r.GetInt32(5)).Returns(() => deliveries[currentIndex].Manifest.Id);
            mockReader.Setup(r => r.GetDecimal(6)).Returns(() => deliveries[currentIndex].Manifest.ItemWeight);
            mockReader.Setup(r => r.GetString(7)).Returns(() => deliveries[currentIndex].Manifest.CustomerName);
            mockReader.Setup(r => r.GetString(8)).Returns(() => deliveries[currentIndex].Manifest.CustomerPhone);
        }
    }
}
