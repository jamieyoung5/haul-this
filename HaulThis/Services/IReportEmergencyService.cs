using HaulThis.Models;

namespace HaulThis.Services
{
  /// <summary>
  /// Provides methods to manage reports of delays and emergencies.
  /// </summary>
  public interface IReportEmergencyService
  {
    /// <summary>
    /// Retrieves all trips asynchronously.
    /// </summary>
    Task<IEnumerable<Trip>> GetTripsByDriverIdAsync(int driverId);

    /// <summary>
    /// Retrieves all reports by trip ID asynchronously.
    /// </summary>
    Task<IEnumerable<Report>> GetReportsByTripIdAsync(int tripId);

    /// <summary>
    /// Retrieves a specific report by its ID asynchronously.
    /// </summary>
    Task<Report> GetReportByIdAsync(int reportId);

    /// <summary>
    /// Adds a new report asynchronously.
    /// </summary>
    Task<int> AddReportAsync(Report report);

    /// <summary>
    /// Updates an existing report asynchronously.
    /// </summary>
    Task<int> UpdateReportAsync(Report report);

    /// <summary>
    /// Deletes a report by ID asynchronously.
    /// </summary>
    Task<int> DeleteReportAsync(int reportId);
  }
}