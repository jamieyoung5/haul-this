using Xunit;
using HaulThis.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using HaulThis.Test.Integration;
using Microsoft.Data.SqlClient;

namespace HaulThis.Test.Integration;
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
        var arrivalTime = DateTime.UtcNow.AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ss");

        string seedQuery = $@"
        
        INSERT INTO TripManifest (ItemId, TripId) VALUES ('123', 'Trip1');
        INSERT INTO Waypoint (Location, ArrivalTime, TripId) VALUES ('New York', '{arrivalTime}', 'Trip1');
    ";
        _databaseService.Execute(seedQuery);
        
        var trackingId = "123";
        
        var trackingInfo = await _trackingService.GetTrackingInfo(trackingId);
        
        Assert.NotNull(trackingInfo);
        Assert.Equal("New York", trackingInfo.CurrentLocation);
        Assert.Equal("In Transit", trackingInfo.Status);
    }

    [Fact]
    public async Task GetTrackingInfo_InvalidTrackingId_ShouldReturnNull()
    {
        var trackingId = "invalid";
        
        var trackingInfo = await _trackingService.GetTrackingInfo(trackingId);
        
        Assert.Null(trackingInfo);
    }

    public void Dispose()
    {
        _databaseSetup.TearDownDatabase(_connection);
        _databaseService.CloseConnection();
    }

}
