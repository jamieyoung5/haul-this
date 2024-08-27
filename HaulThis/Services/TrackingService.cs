using Microsoft.Extensions.Logging;

namespace HaulThis.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<TrackingService> _logger;
        private const string GetTrackingInfoQuery = @"
            SELECT TOP 1
                wp.location, 
                wp.estimatedTime, 
                itm.Id, 
                tm.Id 
            FROM 
                waypoint wp
            INNER JOIN 
                trip tm ON tm.Id = wp.Id
			INNER JOIN 
				item itm ON itm.tripId = wp.tripId
            WHERE 
                tm.Id = @p0
            ORDER BY 
                wp.estimatedTime DESC;";

        public TrackingService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            _logger = loggerFactory.CreateLogger<TrackingService>();
        }
        
        /// <inheritdoc />
        public async Task<TrackingInfo> GetTrackingInfo(int trackingId)
        {
            try
            {
                _logger.LogInformation("Starting GetTrackingInfo for Tracking ID: {TrackingId}", trackingId);

                _logger.LogInformation("Executing query with trackingId: {TrackingId}", trackingId);
                var reader = _databaseService.Query(GetTrackingInfoQuery, trackingId);

                if (reader.Read())
                {
                    _logger.LogInformation("Query returned data for Tracking ID: {TrackingId}", trackingId);

                    var location = reader.GetString(0);
                    var arrivalTime = reader.GetDateTime(1);
                    var eta = CalculateETA(arrivalTime);

                    _logger.LogInformation("Tracking Info - Location: {Location}, Arrival Time: {ArrivalTime}, ETA: {ETA}", location, arrivalTime, eta);

                    reader.Close();
                    return new TrackingInfo
                    {
                        CurrentLocation = location,
                        ETA = eta,
                        Status = eta.HasValue && eta > DateTime.UtcNow ? "In Transit" : "Delivered"
                    };
                }
                else
                {
                    _logger.LogWarning("Query did not return any rows for Tracking ID: {TrackingId}", trackingId);
                }

                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving tracking information for Tracking ID: {TrackingId}", trackingId);
                return null;
            }
        }

        private DateTime? CalculateETA(DateTime lastKnownLocationTime)
        {
            // TODO: Implement actual ETA calculation logic
            var expectedDeliveryTime = lastKnownLocationTime.AddHours(2);

            // If the expected delivery time is in the future, return it as the ETA
            return expectedDeliveryTime > DateTime.UtcNow ? expectedDeliveryTime : null;
        }
    }

    public class TrackingInfo
    {
        public string CurrentLocation { get; set; }
        public DateTime? ETA { get; set; }
        public string Status { get; set; }
    }
}
