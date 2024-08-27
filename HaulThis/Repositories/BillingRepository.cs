using HaulThis.Models;
using HaulThis.Services;

namespace HaulThis.Repositories;

public class BillingRepository(IDatabaseService databaseService) : IBillingRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Bill>> GetBillsByUserAsync(int userId)
    {
        const string getAllBillsByUserIdQuery = """
                                                SELECT Id, userId, amount, billDate, dueDate, status
                                                FROM bill
                                                WHERE userId = @p0;
                                                """;
        
        List<Bill> bills = [];

        using (var reader = databaseService.Query(getAllBillsByUserIdQuery, userId))
        {
            while (reader.Read())
            {
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