using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Services;

namespace HaulThis.Test.Repositories;

public class BillingsRepositoryTests
{
    private readonly Mock<IDatabaseService> _mockDatabaseService;
    private readonly BillingRepository _subject;

    public BillingsRepositoryTests()
    {
        _mockDatabaseService = new Mock<IDatabaseService>();
        _subject = new BillingRepository(_mockDatabaseService.Object);
    }

    [Fact]
    public async Task GetBillsByUserAsync_ReturnsBills_WhenBillsExist()
    {
        // Arrange
        int userId = 1;
        Mock<IDataReader> mockReader = new();

        mockReader.SetupSequence(reader => reader.Read())
            .Returns(true)
            .Returns(true)
            .Returns(false);

        mockReader.Setup(reader => reader.GetInt32(0)).Returns(1);
        mockReader.Setup(reader => reader.GetInt32(1)).Returns(userId);
        mockReader.Setup(reader => reader.GetDecimal(2)).Returns(150.50m);
        mockReader.Setup(reader => reader.GetDateTime(3)).Returns(DateTime.UtcNow.AddDays(-10));
        mockReader.Setup(reader => reader.GetDateTime(4)).Returns(DateTime.UtcNow.AddDays(20));
        mockReader.Setup(reader => reader.GetString(5)).Returns(BillStatus.UNPAID.ToString());

        _mockDatabaseService.Setup(service => service.Query(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns(mockReader.Object);

        // Act
        IEnumerable<Bill> result = await _subject.GetBillsByUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        List<Bill> billsList = result.ToList();
        Assert.Equal(2, billsList.Count);
        Assert.All(billsList, bill => Assert.Equal(userId, bill.UserId));
        Assert.Contains(billsList, bill => bill is { Amount: 150.50m, Status: BillStatus.UNPAID });
    }

    [Fact]
    public async Task GetBillsByUserAsync_ReturnsEmptyList_WhenNoBillsExist()
    {
        // Arrange
        int userId = 1;
        Mock<IDataReader> mockReader = new();

        mockReader.Setup(reader => reader.Read()).Returns(false);

        _mockDatabaseService.Setup(service => service.Query(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns(mockReader.Object);

        // Act
        IEnumerable<Bill> result = await _subject.GetBillsByUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetBillsByUserAsync_ThrowsException_WhenQueryFails()
    {
        // Arrange
        int userId = 1;
        _mockDatabaseService.Setup(service => service.Query(It.IsAny<string>(), It.IsAny<object[]>()))
            .Throws(new Exception("Database query failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _subject.GetBillsByUserAsync(userId));
    }
}