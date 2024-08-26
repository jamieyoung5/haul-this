using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaulThis.Models;

namespace HaulThis.Services
{
  public class ReportEmergencyService : IReportEmergencyService
  {
    // SQL Queries and Statements
    private const string GetAllTripsByDriverIdQuery = "SELECT Id, DriverId, VehicleId, StartDate, EndDate, Status FROM Trips WHERE DriverId = @p0";
    private const string GetAllReportsByTripIdQuery = "SELECT Id, TripId, ReportType, Description, ReportDateTime FROM Reports WHERE TripId = @p0";
    private const string GetReportByIdQuery = "SELECT Id, TripId, ReportType, Description, ReportDateTime FROM Reports WHERE Id = @p0";
    private const string AddReportStmt = """
      INSERT INTO Reports (TripId, ReportType, Description, ReportDateTime)
                          VALUES (@p0, @p1, @p2, @p3)
      """;
    private const string UpdateReportStmt = """
      UPDATE Reports
                      SET TripId = @p0, ReportType = @p1, Description = @p2, ReportDateTime = @p3
                      WHERE Id = @p4
      """;
    private const string DeleteReportStmt = "DELETE FROM Reports WHERE Id = @p0";

    private readonly IDatabaseService databaseService;

    public ReportEmergencyService(IDatabaseService databaseService)
    {
      this.databaseService = databaseService;
    }

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
          });
        }
      }

      return await Task.FromResult(trips);
    }

    public async Task<IEnumerable<Report>> GetReportsByTripIdAsync(int tripId)
    {
      var reports = new List<Report>();

      using (var reader = databaseService.Query(GetAllReportsByTripIdQuery, tripId))
      {
        while (reader.Read())
        {
          reports.Add(new Report
          {
            Id = reader.GetInt32(0),
            TripId = reader.GetInt32(1),
            ReportType = reader.GetString(2),
            Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            ReportDateTime = reader.GetDateTime(4)
          });
        }
      }

      return await Task.FromResult(reports);
    }

    public async Task<Report> GetReportByIdAsync(int reportId)
    {
      var reader = databaseService.QueryRow(GetReportByIdQuery, reportId);

      if (reader.IsDBNull(0))
      {
        throw new InvalidOperationException("Report not found");
      }

      return await Task.FromResult(new Report
      {
        Id = reader.GetInt32(0),
        TripId = reader.GetInt32(1),
        ReportType = reader.GetString(2),
        Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
        ReportDateTime = reader.GetDateTime(4)
      });
    }

    public async Task<int> AddReportAsync(Report report)
    {
      return await Task.FromResult(databaseService.Execute(AddReportStmt, report.TripId, report.ReportType, report.Description, report.ReportDateTime));
    }

    public async Task<int> UpdateReportAsync(Report report)
    {
      return await Task.FromResult(databaseService.Execute(UpdateReportStmt, report.TripId, report.ReportType, report.Description, report.ReportDateTime, report.Id));
    }

    public async Task<int> DeleteReportAsync(int reportId)
    {
      return await Task.FromResult(databaseService.Execute(DeleteReportStmt, reportId));
    }
  }
}
