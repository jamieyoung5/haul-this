using System;
using HaulThis.Models;

namespace HaulThis.Services;

public class ManageExpensesService(IDatabaseService databaseService) : IManageExpensesService
{
    private const string GetAllTripsByDriverIdQuery = "SELECT t.Id, t.DriverId, t.VehicleId, t.StartDate, t.EndDate, t.Status FROM trip t WHERE t.DriverId = @p0";
    private const string GetAllExpensesByTripIdQuery = "SELECT e.Id, e.TripId, e.Amount, e.Description, e.Date FROM expense e WHERE e.TripId = @p0";

    private const string GetExpenseByIdQuery = "SELECT e.Id, e.TripId, e.Amount, e.Description, e.Date FROM expense e WHERE e.Id = @p0";

    private const string AddExpenseStmt = """
                                        INSERT INTO expense (TripId, Amount, Description, Date)
                                                              VALUES (@p0, @p1, @p2, @p3)
                                        """;

    private const string UpdateExpenseStmt = """
                                           UPDATE expense 
                                                                 SET TripId = @p0, Amount = @p1, Description = @p2, Date = @p3
                                                                 WHERE Id = @p4
                                           """;

    private const string DeleteExpenseStmt = "DELETE FROM expense WHERE id = @p0";

    public async Task<IEnumerable<Trip>> GetTripsByDriverIdAsync(int driverId)
    {
        var trips = new List<Trip>();

        using (var reader = databaseService.Query(GetAllTripsByDriverIdQuery, driverId))
        {
            while (reader.Read())
            {
                trips.Add(new Trip
                {
                    Id = reader.GetInt32(0),
                    DriverId = reader.GetInt32(1),
                    VehicleId = reader.GetInt32(2),
                    StartDate = reader.GetDateTime(5),
                    EndDate = reader.GetDateTime(6),
                    Status = reader.GetString(7),
                    
                });
            }
        }

        return await Task.FromResult(trips);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByTripIdAsync(int tripId)
    {
        var expenses = new List<Expense>();

        using (var reader = databaseService.Query(GetAllExpensesByTripIdQuery, tripId))
        {
            while (reader.Read())
            {
                expenses.Add(new Expense
                {
                    Id = reader.GetInt32(0),
                    TripId = reader.GetInt32(1),
                    Amount = reader.GetDecimal(2),
                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    Date = reader.GetDateTime(4)
                });
            }
        }

        return await Task.FromResult(expenses);
    }

    public async Task<Expense> GetExpenseByIdAsync(int expenseId)
    {
        var reader = databaseService.QueryRow(GetExpenseByIdQuery, expenseId);

        if (reader.IsDBNull(0))
        {
            throw new InvalidOperationException("Expense not found");
        }

        return await Task.FromResult(new Expense
        {
            Id = reader.GetInt32(0),
            TripId = reader.GetInt32(1),
            Amount = reader.GetDecimal(2),
            Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            Date = reader.GetDateTime(4)
        });
    }

    public async Task<int> AddExpenseAsync(Expense expense)
    {
        return await Task.FromResult(databaseService.Execute(AddExpenseStmt, expense.TripId, expense.Amount, expense.Description, expense.Date));
    }

    public async Task<int> UpdateExpenseAsync(Expense expense)
    {
        return await Task.FromResult(databaseService.Execute(UpdateExpenseStmt, expense.TripId, expense.Amount, expense.Description, expense.Date, expense.Id));
    }

    public async Task<int> DeleteExpenseAsync(int expenseId)
    {
        return await Task.FromResult(databaseService.Execute(DeleteExpenseStmt, expenseId));
    }





}


