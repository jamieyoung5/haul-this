using System.Collections.ObjectModel;
using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.ViewModels;

namespace HaulThis.Test.ViewModels;

public class BillingListViewModelTests
{
    private readonly Mock<IBillingRepository> _mockBillingRepository;
    private readonly BillingListViewModel _subject;

    public BillingListViewModelTests()
    {
        _mockBillingRepository = new Mock<IBillingRepository>();
        _subject = new BillingListViewModel(_mockBillingRepository.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeBillsCollection()
    {
        Assert.NotNull(_subject.Items);
        Assert.IsType<ObservableCollection<Bill>>(_subject.Items);
    }

    [Fact]
    public void Constructor_ShouldCallLoadBills()
    {
        _mockBillingRepository.Verify(repo => repo.GetBillsByUserAsync(1), Times.Once);
    }

    [Fact]
    public void LoadBills_ShouldUpdateBillsCollection()
    {
        // Arrange
        List<Bill> bills = new List<Bill>
        {
            new()
            {
                Id = 1, UserId = 1, Amount = 100.00m, BillDate = DateTime.UtcNow.AddDays(-10),
                DueDate = DateTime.UtcNow.AddDays(20), Status = BillStatus.UNPAID
            },
            new()
            {
                Id = 2, UserId = 1, Amount = 150.00m, BillDate = DateTime.UtcNow.AddDays(-5),
                DueDate = DateTime.UtcNow.AddDays(15), Status = BillStatus.PAID
            }
        };
        _mockBillingRepository.Setup(repo => repo.GetBillsByUserAsync(1)).ReturnsAsync(bills);

        // Act
        var result = new BillingListViewModel(_mockBillingRepository.Object);

        // Assert
        Assert.Equal(bills.Count, result.Items.Count);
        for (int i = 0; i < bills.Count; i++)
        {
            Assert.Equal(bills[i].Id, result.Items[i].Id);
            Assert.Equal(bills[i].Amount, result.Items[i].Amount);
            Assert.Equal(bills[i].Status, result.Items[i].Status);
        }
    }
}