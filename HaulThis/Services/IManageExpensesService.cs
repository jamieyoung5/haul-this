using System;
using HaulThis.Models;

namespace HaulThis.Services;

/// <summary>
/// Interface defining expense management operations.
/// </summary>
public interface IManageExpensesService
{   
    /// <summary>
    /// Retrieves all trips asynchronously.
    /// </summary>
    Task<IEnumerable<Trip>> GetTripsByDriverIdAsync(int driverId);

    /// <summary>
    /// Retrieves all expenses asynchronously.
    /// </summary>
    Task<IEnumerable<Expense>> GetExpensesByTripIdAsync(int tripId);

    /// <summary>
    /// Retrieves a specific expense by its ID asynchronously.
    /// </summary>
    Task<Expense> GetExpenseByIdAsync(int expenseId);

    /// <summary>
    /// Adds a new expense asynchronously.
    /// </summary>
    Task<int> AddExpenseAsync(Expense expense);

    /// <summary>
    /// Updates an existing expense asynchronously.
    /// </summary>
    Task<int> UpdateExpenseAsync(Expense expense);

    /// <summary>
    /// Deletes an expense by ID asynchronously.
    /// </summary>
    Task<int> DeleteExpenseAsync(int expenseId);
}
