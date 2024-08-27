using HaulThis.Models;

namespace HaulThis.Services;

public class BillingService(IDatabaseService databaseService) : IBillingService
{
    private const string GetAllBillsByUserIdQuery = "SELECT Id, userId, amount, billDate, dueDate, status FROM bill WHERE userId = @p0";

    /// <inheritdoc />
    public async Task<IEnumerable<Bill>> GetBillingInfoByUserAsync(int userId)
    {
        List<Bill> bills = [];

        using (var reader = databaseService.Query(GetAllBillsByUserIdQuery, userId))
        {
            while (reader.Read())
            {
                var testId = reader.GetInt32(0);
                var testUserId = reader.GetInt32(1);
                var testAmount = reader.GetDecimal(2);
                var testBillDate = reader.GetDateTime(3);
                var testDueDate = reader.GetDateTime(4);
                var testStatus = Enum.Parse<BillStatus>(reader.GetString(5));
                
                bills.Add(new Bill
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Amount = reader.GetDecimal(2),
                    BillDate = reader.GetDateTime(3),
                    DueDate = reader.GetDateTime(4),
                    Status = Enum.Parse<BillStatus>(reader.GetString(5)),
                });
            }
        }
        
        return await Task.FromResult(bills);
    }
}