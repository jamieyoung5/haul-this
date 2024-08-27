using HaulThis.Models;
using HaulThis.Repositories;

namespace HaulThis.Test.Integration;

[Collection("Sequential Tests")]
public class ManageBillingTest : DisposableIntegrationTest
{
    private readonly IBillingRepository _billingRepository;

    public ManageBillingTest()
    {
        _billingRepository = new BillingRepository(_databaseService);
    }

    [Fact]
    public async Task GetBillsByUserAsync_ReturnsBills_WhenBillsExistForUser()
    {
        // Arrange
        int userId = InsertTestUser();
        var bill = new Bill
        {
            UserId = userId,
            Amount = 100.00m,
            BillDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(30),
            Status = BillStatus.UNPAID
        };

        InsertTestBill(bill);

        // Act
        IEnumerable<Bill> bills = await _billingRepository.GetBillsByUserAsync(userId);

        // Assert
        Assert.NotNull(bills);
        Assert.Single(bills);
        Assert.Equal(userId, bills.First().UserId);
        Assert.Equal(100.00m, bills.First().Amount);
        Assert.Equal(BillStatus.UNPAID, bills.First().Status);
    }

    [Fact]
    public async Task GetBillsByUserAsync_ReturnsEmptyList_WhenNoBillsExistForUser()
    {
        // Arrange
        int userId = InsertTestUser();

        // Act
        IEnumerable<Bill> bills = await _billingRepository.GetBillsByUserAsync(userId);

        // Assert
        Assert.NotNull(bills);
        Assert.Empty(bills);
    }

    [Fact]
    public async Task GetBillsByUserAsync_HandlesMultipleBillsCorrectly()
    {
        // Arrange
        int userId = InsertTestUser();
        var bill1 = new Bill
        {
            UserId = userId,
            Amount = 150.00m,
            BillDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(30),
            Status = BillStatus.UNPAID
        };

        var bill2 = new Bill
        {
            UserId = userId,
            Amount = 200.00m,
            BillDate = DateTime.UtcNow.AddDays(-15),
            DueDate = DateTime.UtcNow.AddDays(15),
            Status = BillStatus.PAID
        };

        InsertTestBill(bill1);
        InsertTestBill(bill2);

        // Act
        IEnumerable<Bill> bills = await _billingRepository.GetBillsByUserAsync(userId);

        // Assert
        Assert.NotNull(bills);
        Assert.Equal(2, bills.Count());
        Assert.Contains(bills, b => b.Amount == 150.00m && b.Status == BillStatus.UNPAID);
        Assert.Contains(bills, b => b.Amount == 200.00m && b.Status == BillStatus.PAID);
    }

    private int InsertTestUser()
    {
        _databaseService.Execute(
            "INSERT INTO users (roleId, firstName, lastName, email, phoneNumber, address, createdAt) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)",
            1, "Test", "User", "test.user@example.com", "1234567890", "123 Test St", DateTime.UtcNow);

        return _databaseService.QueryRow("SELECT Id FROM users WHERE email = @p0", "test.user@example.com").GetInt32(0);
    }

    private void InsertTestBill(Bill bill)
    {
        _databaseService.Execute(
            "INSERT INTO bill (userId, amount, billDate, dueDate, status) VALUES (@p0, @p1, @p2, @p3, @p4)",
            bill.UserId, bill.Amount, bill.BillDate, bill.DueDate, bill.Status.ToString());
    }
}