using Xunit;
using HaulThis.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

public class TrackingServiceIntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDbConnection _connection;

    public TrackingServiceIntegrationTests()
    {
        var serviceCollection = new ServiceCollection();

        // Create an in-memory database connection
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();  // Keep the connection open for the lifetime of the test

        // Register the in-memory SQLite database connection as a singleton
        serviceCollection.AddSingleton(_connection);

        // Register the existing DatabaseService and other services
        serviceCollection.AddTransient<IDatabaseService, DatabaseService>();
        serviceCollection.AddTransient<ITrackingService, TrackingService>();

        // Add logging
        serviceCollection.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });

        // Build the service provider
        _serviceProvider = serviceCollection.BuildServiceProvider();

        // Seed the in-memory database using the existing DatabaseService
        var databaseService = _serviceProvider.GetService<IDatabaseService>();
        SeedDatabase(databaseService);
    }

    private void SeedDatabase(IDatabaseService databaseService)
{   
     var arrivalTime = DateTime.UtcNow.AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ss");

    string seedQuery = $@"
        CREATE TABLE IF NOT EXISTS TripManifest (ItemId TEXT, TripId TEXT);
        CREATE TABLE IF NOT EXISTS Waypoint (Location TEXT, ArrivalTime DATETIME, TripId TEXT);
        INSERT INTO TripManifest (ItemId, TripId) VALUES ('123', 'Trip1');
        INSERT INTO Waypoint (Location, ArrivalTime, TripId) VALUES ('New York', '{arrivalTime}', 'Trip1');
    ";

    databaseService.Execute(seedQuery);
}


    [Fact]
    public async Task GetTrackingInfo_ValidTrackingId_ShouldReturnTrackingInfo()
    {
        // Arrange
        var trackingService = _serviceProvider.GetService<ITrackingService>();
        var trackingId = "123";

        // Act
        var trackingInfo = await trackingService.GetTrackingInfo(trackingId);

        // Assert
        Assert.NotNull(trackingInfo);
        Assert.Equal("New York", trackingInfo.CurrentLocation);
        Assert.Equal("In Transit", trackingInfo.Status);
    }

    [Fact]
    public async Task GetTrackingInfo_InvalidTrackingId_ShouldReturnNull()
    {
        // Arrange
        var trackingService = _serviceProvider.GetService<ITrackingService>();
        var trackingId = "invalid";

        // Act
        var trackingInfo = await trackingService.GetTrackingInfo(trackingId);

        // Assert
        Assert.Null(trackingInfo);
    }

    public void Dispose()
    {
        _connection.Close(); 
    }
}
