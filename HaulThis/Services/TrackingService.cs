using System;
using System.Threading.Tasks;
using HaulThis.Models;
using Microsoft.Extensions.Logging;

namespace HaulThis.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<TrackingService> _logger;

       public TrackingService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Create a logger instance
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            _logger = loggerFactory.CreateLogger<TrackingService>();
        }

        public async Task<TrackingInfo> GetTrackingInfo(string trackingId)
        {
            try
            {
                
                if (!_databaseService.CreateConnection())
                {
                    _logger.LogError("Failed to create a database connection.");
                    return null;
                }

                string query = @"
                    SELECT 
                        wp.Location, 
                        wp.ArrivalTime, 
                        tm.ItemId, 
                        tm.TripId 
                    FROM 
                        Waypoint wp
                    INNER JOIN 
                        TripManifest tm ON tm.TripId = wp.TripId
                    WHERE 
                        tm.ItemId = @p0
                    ORDER BY 
                        wp.ArrivalTime DESC
                    LIMIT 1";

                using var reader = _databaseService.Query(query, trackingId);

                if (reader.Read())
                {
                    var location = reader.GetString(0);
                    var arrivalTime = reader.GetDateTime(1);
                    var eta = CalculateETA(arrivalTime);

                    return new TrackingInfo
                    {
                        CurrentLocation = location,
                        ETA = eta,
                        Status = eta.HasValue && eta > DateTime.UtcNow ? "In Transit" : "Delivered"
                    };
                }

                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving tracking information.");
                return null;
            }
            finally
            {
                _databaseService.CloseConnection();
            }
        }

        private DateTime? CalculateETA(DateTime lastKnownLocationTime)
        {
            // Example logic for calculating ETA based on the last known location time
            // This is a placeholder; actual implementation would depend on business requirements
            return lastKnownLocationTime.AddHours(2);
        }
    }

    public class TrackingInfo
    {
        public string CurrentLocation { get; set; }
        public DateTime? ETA { get; set; }
        public string Status { get; set; }
    }
}
