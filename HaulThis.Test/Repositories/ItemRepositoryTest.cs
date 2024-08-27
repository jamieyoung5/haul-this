using HaulThis.Repositories;
using HaulThis.Services;

namespace HaulThis.Test.Repositories;

public class ItemRepositoryTests
{
    private readonly Mock<IDatabaseService> _mockDatabaseService;
    private readonly ItemRepository _subject;

    public ItemRepositoryTests()
    {
        _mockDatabaseService = new Mock<IDatabaseService>();
        _subject = new ItemRepository(_mockDatabaseService.Object);
    }

    [Fact]
    public async Task MarkItemAsDelivered_WhenItemDoesNotExist_ShouldReturnZeroRowsAffected()
    {
        // Arrange
        int tripId = 1;
        int itemId = 999; // Non-existent item
        _mockDatabaseService.Setup(ds => ds.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(0);

        // Act
        int result = await _subject.MarkAsDeliveredAsync(tripId, itemId);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task MarkItemAsDelivered_WhenMultipleItemsAreDelivered_ShouldReturnNumberOfRowsAffected()
    {
        // Arrange
        int tripId = 1;
        int itemId1 = 1;
        int itemId2 = 2;

        _mockDatabaseService.Setup(ds => ds.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);

        // Act
        int result1 = await _subject.MarkAsDeliveredAsync(tripId, itemId1);
        int result2 = await _subject.MarkAsDeliveredAsync(tripId, itemId2);

        // Assert
        Assert.Equal(1, result1);
        Assert.Equal(1, result2);
        Assert.Equal(2, result1 + result2);
    }
}