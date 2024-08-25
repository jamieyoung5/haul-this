using System;
using System.Threading.Tasks;
using HaulThis.Services;
using HaulThis.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Xunit;

namespace HaulThis.Test.Integration
{
    public class TrackingServiceIntegrationTests : IDisposable
    {
        private readonly ITrackingService _trackingService;
        private readonly SqlConnection _connection;
        private readonly DatabaseSetup _databaseSetup;
        private readonly IDatabaseService _databaseService;

        public TrackingServiceIntegrationTests()
        {
            _databaseSetup = new DatabaseSetup();
            _connection = _databaseSetup.DeployDatabase();

            var loggerFactory = LoggerFactory.Create(loggerBuilder =>
            {
                loggerBuilder.AddConsole();
                loggerBuilder.AddDebug();
            });
            ILogger<DatabaseService> logger = loggerFactory.CreateLogger<DatabaseService>();
            _databaseService = new DatabaseService(_connection, logger);
            _trackingService = new TrackingService(_databaseService);
        }

        [Fact]
        public async Task GetTrackingInfo_ValidTrackingId_ShouldReturnTrackingInfo()
        {
            // Arrange: Seed the database with test data
            var trackingId = SeedTestData();

            // Act: Call the service method
            var trackingInfo = await _trackingService.GetTrackingInfo(trackingId.ToString());

            // Assert: Verify the result
            Assert.NotNull(trackingInfo);
            Assert.Equal("New York", trackingInfo.CurrentLocation);
            Assert.Equal("In Transit", trackingInfo.Status);
        }

        [Fact]
        public async Task GetTrackingInfo_InvalidTrackingId_ShouldReturnNull()
        {
            // Arrange: Use a trackingId that does not exist
            var invalidTrackingId = "99999"; // Assuming this ID does not exist in your test data

            // Act: Call the service method with an invalid tracking ID
            var trackingInfo = await _trackingService.GetTrackingInfo(invalidTrackingId);

            // Assert: Verify that the result is null
            Assert.Null(trackingInfo);
        }

        [Fact]
        public async Task GetTrackingInfo_EmptyTrackingId_ShouldReturnNull()
        {
            // Arrange: Use an empty string as trackingId
            var emptyTrackingId = "";

            // Act: Call the service method with an empty tracking ID
            var trackingInfo = await _trackingService.GetTrackingInfo(emptyTrackingId);

            // Assert: Verify that the result is null
            Assert.Null(trackingInfo);
        }

        [Fact]
        public async Task GetTrackingInfo_NullTrackingId_ShouldReturnNull()
        {
            // Arrange: Use null as trackingId
            string nullTrackingId = null;

            // Act: Call the service method with a null tracking ID
            var trackingInfo = await _trackingService.GetTrackingInfo(nullTrackingId);

            // Assert: Verify that the result is null
            Assert.Null(trackingInfo);
        }

       private int SeedTestData()
{
    // Define the current time
    var currentTime = DateTime.UtcNow;

    // Seed data into the database
    string seedQuery = $@"
        -- Insert into users table
        INSERT INTO users (roleId, firstName, lastName, email, phoneNumber, address, createdAt) 
        VALUES (1, 'John', 'Doe', 'john.doe@example.com', '1234567890', '123 Main St', '{currentTime:yyyy-MM-dd HH:mm:ss}');
        DECLARE @driverId INT = SCOPE_IDENTITY();

        -- Insert into vehicle table
        INSERT INTO vehicle (make, model, year, licensePlate, status, createdAt) 
        VALUES ('Austin', 'Mini', 1980, 'XYZ123', 'Available', '{currentTime:yyyy-MM-dd HH:mm:ss}');
        DECLARE @vehicleId INT = SCOPE_IDENTITY();

        -- Insert into trip table
        INSERT INTO trip (vehicleId, driverId, startDate) 
        VALUES (@vehicleId, @driverId, '{currentTime:yyyy-MM-dd HH:mm:ss}');
        DECLARE @tripId INT = SCOPE_IDENTITY();

        -- Insert into item table
        INSERT INTO item (tripId, billId, goodsCategoryId, pickedUpBy, deliveredBy, pickupDate, deliveryDate) 
        VALUES (@tripId, NULL, NULL, NULL, NULL, '{currentTime:yyyy-MM-dd HH:mm:ss}', '{currentTime.AddHours(1):yyyy-MM-dd HH:mm:ss}');
        DECLARE @itemId INT = SCOPE_IDENTITY();

        -- Insert into tripManifest table
        INSERT INTO tripManifest (tripId, itemId, pickupRequestId, deliveryRequestId) 
        VALUES (@tripId, @itemId, NULL, NULL);
        DECLARE @trackingId INT = SCOPE_IDENTITY();

        -- Insert into waypoint table
        INSERT INTO waypoint (location, arrivalTime, tripId) 
        VALUES ('New York', '{currentTime:yyyy-MM-dd HH:mm:ss}', @tripId);

        SELECT @trackingId;
    ";

    
    using (var command = _connection.CreateCommand())
    {
        command.CommandText = seedQuery;
        var trackingId = (int)command.ExecuteScalar(); 
        return trackingId;
    }
}


        public void Dispose()
        {
            _databaseSetup.TearDownDatabase(_connection);
            _databaseService.CloseConnection(); 
        }
    }
}
