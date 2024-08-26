using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Test.ViewModels;

public class TripListViewModelTests
{
    private readonly Mock<ITripService> _mockTripService;
    private readonly TripListViewModel _subject;

    public TripListViewModelTests()
    {
        _mockTripService = new Mock<ITripService>();
        _subject = new TripListViewModel(_mockTripService.Object);
    }
    
    [Fact]
    public void Constructor_ShouldInitializeUsersCollection()
    {
        Assert.NotNull(_subject.Trips);
        Assert.IsType<ObservableCollection<Trip>>(_subject.Trips);
    }

    [Fact]
    public void Constructor_ShouldCallLoadUsers()
    {
        _mockTripService.Verify(s => s.GetTripByDateAsync(It.IsAny<DateTime>()), Times.Once);
    }
    
    [Fact]
    public void LoadTrips_ShouldUpdateTripsCollection()
    {
        // Arrange
        var trips = new List<Trip>
        {
            new Trip { Id = 1, Vehicle = new Vehicle { LicensePlate = "Truck A" }, Driver = new User { FirstName = "John" } },
            new Trip { Id = 2, Vehicle = new Vehicle { LicensePlate = "Truck B" }, Driver = new User { FirstName = "Jane" } }
        };
        _mockTripService.Setup(s => s.GetTripByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(trips);
        
        // Act
        var result = new TripListViewModel(_mockTripService.Object);
        
        // Assert
        Assert.Equal(trips.Count, result.Trips.Count);
        for (int i = 0; i < trips.Count; i++)
        {
            Assert.Equal(trips[i].Id, result.Trips[i].Id);
            Assert.Equal(trips[i].Vehicle.LicensePlate, result.Trips[i].Vehicle.LicensePlate);
            Assert.Equal(trips[i].Driver.FirstName, result.Trips[i].Driver.FirstName);
        }
    }
}