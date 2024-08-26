using System;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Test.ViewModels;

public class ManageExpensesViewModelTests
{
    private readonly Mock<IManageExpensesService> _mockService;
    private readonly ManageExpensesViewModel _viewModel;

    public ManageExpensesViewModelTests()
    {
        _mockService = new Mock<IManageExpensesService>();
        _viewModel = new ManageExpensesViewModel(_mockService.Object);
    }

    [Fact]
    public async Task LoadTripsFromDriver_LoadsTrips()
    {
        // Arrange
        var driver = new User
        {
            Id = 123,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            Address = "123 Main St"
        };

        var vehicle = new Vehicle
        {
            Id = 456,
            // Add other necessary properties for Vehicle
        };

        var trips = new List<Trip>
    {
        new Trip
        {
            Id = 1,
            Driver = driver,
            Vehicle = vehicle,
            TripManifest = new List<Delivery>(),
        }
    };

        _mockService.Setup(s => s.GetTripsByDriverIdAsync(It.IsAny<int>())).ReturnsAsync(trips);

        _viewModel.DriverId = "123";

        // Act
        await _viewModel.LoadTripsFromDriver();

        // Assert
        Assert.Single(_viewModel.Trips);
        var trip = _viewModel.Trips[0];
        Assert.Equal(1, trip.Id);
        Assert.Equal(123, trip.Driver.Id);
        Assert.Equal(456, trip.Vehicle.Id);
    }


    [Fact]
    public async Task LoadTripExpenses_LoadsExpenses()
    {
        // Arrange
        var expenses = new List<Expense>
        {
            new Expense { Id = 1, TripId = 100, Amount = 50.00m, Description = "Food" }
        };

        _mockService.Setup(s => s.GetExpensesByTripIdAsync(It.IsAny<int>())).ReturnsAsync(expenses);

        _viewModel.SelectedTrip = new Trip { Id = 100 };

        // Act
        await _viewModel.LoadTripExpenses();

        // Assert
        Assert.Single(_viewModel.Expenses);
        var expense = _viewModel.Expenses[0];
        Assert.Equal(100, expense.TripId);
        Assert.Equal("Food", expense.Description);
    }

    [Fact]
    public void DriverId_SetProperty_UpdatesDriverId()
    {
        // Arrange
        var driverId = "456";

        // Act
        _viewModel.DriverId = driverId;

        // Assert
        Assert.Equal(driverId, _viewModel.DriverId);
    }

    [Fact]
    public void SelectedTrip_SetProperty_UpdatesSelectedTrip()
    {
        // Arrange
        var driver = new User
        {
            Id = 123,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890",
            Address = "123 Main St"
        };

        var vehicle = new Vehicle
        {
            Id = 456,
            // Add other necessary properties for Vehicle
        };

        var trip = new Trip
        {
            Id = 1,
            Driver = driver,
            Vehicle = vehicle,
            TripManifest = new List<Delivery>()
        };

        // Act
        _viewModel.SelectedTrip = trip;

        // Assert
        Assert.Equal(trip, _viewModel.SelectedTrip);
        Assert.Equal(123, _viewModel.SelectedTrip.Driver.Id);
        Assert.Equal(456, _viewModel.SelectedTrip.Vehicle.Id);
    }

}
