using System;
using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.Test.Services;

public class ManageExpensesServiceTests
{
    private readonly Mock<IDatabaseService> _mockDatabaseService;
    private readonly ManageExpensesService _service;

    public ManageExpensesServiceTests()
    {
        _mockDatabaseService = new Mock<IDatabaseService>();
        _service = new ManageExpensesService(_mockDatabaseService.Object);
    }

    [Fact]
    public async Task GetTripsByDriverIdAsync_ReturnsTrips()
    {
        // Arrange
        var mockReader = new Mock<IDataReader>();
        mockReader.SetupSequence(r => r.Read())
                  .Returns(true)
                  .Returns(false);

        mockReader.Setup(r => r.GetInt32(0)).Returns(1);
        mockReader.Setup(r => r.GetInt32(1)).Returns(123);
        mockReader.Setup(r => r.GetInt32(2)).Returns(456);
        mockReader.Setup(r => r.GetDateTime(5)).Returns(DateTime.Now);
        mockReader.Setup(r => r.GetDateTime(6)).Returns(DateTime.Now);
        mockReader.Setup(r => r.GetString(7)).Returns("Completed");

        _mockDatabaseService.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
                            .Returns(mockReader.Object);

        // Act
        var result = await _service.GetTripsByDriverIdAsync(123);

        // Assert
        Assert.Single(result);
        var trip = result.First();
        Assert.Equal(123, trip.DriverId);
        Assert.Equal(456, trip.VehicleId);
        Assert.Equal("Completed", trip.Status);
    }

    [Fact]
    public async Task GetExpensesByTripIdAsync_ReturnsExpenses()
    {
        // Arrange
        var mockReader = new Mock<IDataReader>();
        mockReader.SetupSequence(r => r.Read())
                  .Returns(true)
                  .Returns(false);

        mockReader.Setup(r => r.GetInt32(0)).Returns(1);
        mockReader.Setup(r => r.GetInt32(1)).Returns(100);
        mockReader.Setup(r => r.GetDecimal(2)).Returns(50.75m);
        mockReader.Setup(r => r.GetString(3)).Returns("Fuel");
        mockReader.Setup(r => r.GetDateTime(4)).Returns(DateTime.Now);

        _mockDatabaseService.Setup(db => db.Query(It.IsAny<string>(), It.IsAny<object[]>()))
                            .Returns(mockReader.Object);

        // Act
        var result = await _service.GetExpensesByTripIdAsync(100);

        // Assert
        Assert.Single(result);
        var expense = result.First();
        Assert.Equal(100, expense.TripId);
        Assert.Equal(50.75m, expense.Amount);
        Assert.Equal("Fuel", expense.Description);
    }

    [Fact]
    public async Task GetExpenseByIdAsync_ExpenseExists_ReturnsExpense()
    {
        // Arrange
        var mockReader = new Mock<IDataReader>();
        mockReader.Setup(r => r.IsDBNull(0)).Returns(false);
        mockReader.Setup(r => r.GetInt32(0)).Returns(1);
        mockReader.Setup(r => r.GetInt32(1)).Returns(100);
        mockReader.Setup(r => r.GetDecimal(2)).Returns(25.50m);
        mockReader.Setup(r => r.GetString(3)).Returns("Meal");
        mockReader.Setup(r => r.GetDateTime(4)).Returns(DateTime.Now);

        _mockDatabaseService.Setup(db => db.QueryRow(It.IsAny<string>(), It.IsAny<object[]>()))
                            .Returns(mockReader.Object);

        // Act
        var result = await _service.GetExpenseByIdAsync(1);

        // Assert
        Assert.Equal(100, result.TripId);
        Assert.Equal(25.50m, result.Amount);
        Assert.Equal("Meal", result.Description);
    }

    [Fact]
    public async Task GetExpenseByIdAsync_ExpenseNotFound_ThrowsException()
    {
        // Arrange
        var mockReader = new Mock<IDataReader>();
        mockReader.Setup(r => r.IsDBNull(0)).Returns(true);

        _mockDatabaseService.Setup(db => db.QueryRow(It.IsAny<string>(), It.IsAny<object[]>()))
                            .Returns(mockReader.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetExpenseByIdAsync(1));
    }

    [Fact]
    public async Task AddExpenseAsync_CallsExecute()
    {
        // Arrange
        var expense = new Expense { TripId = 100, Amount = 75.00m, Description = "Lodging", Date = DateTime.Now };

        _mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
                            .Returns(1);

        // Act
        var result = await _service.AddExpenseAsync(expense);

        // Assert
        Assert.Equal(1, result);
        _mockDatabaseService.Verify(db => db.Execute(It.IsAny<string>(), expense.TripId, expense.Amount, expense.Description, expense.Date), Times.Once);
    }

    [Fact]
    public async Task UpdateExpenseAsync_CallsExecute()
    {
        // Arrange
        var expense = new Expense { Id = 1, TripId = 100, Amount = 50.00m, Description = "Food", Date = DateTime.Now };

        _mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
                            .Returns(1);

        // Act
        var result = await _service.UpdateExpenseAsync(expense);

        // Assert
        Assert.Equal(1, result);
        _mockDatabaseService.Verify(db => db.Execute(It.IsAny<string>(), expense.TripId, expense.Amount, expense.Description, expense.Date, expense.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteExpenseAsync_CallsExecute()
    {
        // Arrange
        _mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>()))
                            .Returns(1);

        // Act
        var result = await _service.DeleteExpenseAsync(1);

        // Assert
        Assert.Equal(1, result);
        _mockDatabaseService.Verify(db => db.Execute(It.IsAny<string>(), 1), Times.Once);
    }
}
