using HaulThis.Models;
using HaulThis.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace HaulThis.Test.Integration;

public class ManageVehiclesTest : IDisposable
{
    private readonly IManageVehiclesService _manageVehiclesService;
    private readonly IDatabaseService _databaseService;
    private readonly SqlConnection _connection;
    private readonly DatabaseSetup _databaseSetup;
    
    public ManageVehiclesTest()
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
        _manageVehiclesService = new ManageVehiclesService(_databaseService);
    }

    [Fact]
    public async Task AddVehicle_ShouldAddVehicleToDatabase_WhenValidVehicleIsProvided()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Make = "Austin",
            Model = "Mini",
            Year = 1980,
            LicensePlate = "ABC123",
            CreatedAt = DateTime.UtcNow
        };
        
        // Act
        int result = await _manageVehiclesService.AddVehicleAsync(vehicle);
        
        // Assert
        var vehicles = await _manageVehiclesService.GetAllVehiclesAsync();
        Assert.Equal(1, result);
        Assert.Contains(vehicles, v => v.LicensePlate == "ABC123");
    }

    [Fact]
    public async Task DeleteVehicle_ShouldRemoveVehicleFromDatabase_WhenVehicleIdExists()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Make = "Porsche",
            Model = "Boxster",
            Year = 2019,
            LicensePlate = "XYZ789",
            CreatedAt = DateTime.UtcNow
        };
        
        int vehicleId = await _manageVehiclesService.AddVehicleAsync(vehicle);
        var vehiclesBeforeDeletion = await _manageVehiclesService.GetAllVehiclesAsync();
        Assert.Contains(vehiclesBeforeDeletion, v => v.LicensePlate == "XYZ789");
        
        // Act
        int result = await _manageVehiclesService.DeleteVehicleAsync(vehicleId);
        
        // Assert
        var vehiclesAfterDeletion = await _manageVehiclesService.GetAllVehiclesAsync();
        Assert.Equal(1, result);
        Assert.DoesNotContain(vehiclesAfterDeletion, v => v.LicensePlate == "XYZ789");
    }
    
    [Fact]
    public async Task UpdateVehicle_ShouldModifyVehicleInDatabase_WhenEditsAreValid()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Make = "Ford",
            Model = "Focus",
            Year = 2018,
            LicensePlate = "LMN456",
            CreatedAt = DateTime.UtcNow
        };
    
        int vehicleId = await _manageVehiclesService.AddVehicleAsync(vehicle);
        var vehicleToUpdate = await _manageVehiclesService.GetVehicleByIdAsync(vehicleId);
        vehicleToUpdate.Model = "Focus ST";
        
        // Act
        int result = await _manageVehiclesService.UpdateVehicleAsync(vehicleToUpdate);
        
        // Assert
        var updatedVehicle = await _manageVehiclesService.GetVehicleByIdAsync(vehicleId);
        Assert.Equal(1, result);
        Assert.Equal("Focus ST", updatedVehicle.Model);
    }

    public void Dispose()
    {
        _databaseSetup.TearDownDatabase(_connection);
        _databaseService.CloseConnection(); 
    }
}
