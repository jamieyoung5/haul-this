using HaulThis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaulThis.Services;

public class ManageVehiclesService(IDatabaseService databaseService) : IManageVehiclesService
{
    private const string GetAllVehiclesQuery = "SELECT v.uniqueId, v.make, v.model, v.year, v.licensePlate, v.status, v.createdAt, v.updatedAt FROM vehicle v";
    private const string GetAllVehiclesByIdQuery = "SELECT v.uniqueId, v.make, v.model, v.year, v.licensePlate, v.status, v.createdAt, v.updatedAt FROM vehicle v WHERE v.uniqueId = @p0";

    private const string AddVehicleStmt = """
                                        INSERT INTO vehicle (make, model, year, licensePlate, status, createdAt)
                                                              VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
                                        """;
    private const string UpdateVehicleStmt = """
                                           UPDATE vehicle 
                                                                 SET make = @p0, model = @p1, year = @p2, licensePlate = @p3, status = @p4, updatedAt = @p5
                                                                 WHERE uniqueId = @p6
                                           """;
    private const string DeleteVehicleStmt = "DELETE FROM vehicle WHERE uniqueId = @p0";
    
    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
    {
        var vehicles = new List<Vehicle>();

        using (var reader = databaseService.Query(GetAllVehiclesQuery))
        {
            while (reader.Read())
            {   
                
                System.Console.WriteLine(" id: " + reader.GetInt32(0) + " make: " + reader.GetString(1) + " model: " + reader.GetString(2) + " year: " + reader.GetInt32(3) + " licensePlate: " + reader.GetString(4) + " status: " + reader.GetString(5) + " createdAt: " + reader.GetDateTime(6) + " updatedAt: " + (reader.IsDBNull(7) ? "NULL" : reader.GetDateTime(7).ToString()));

                vehicles.Add(new Vehicle
                {
                    Id = reader.GetInt32(0),
                    Make = reader.GetString(1),
                    Model = reader.GetString(2),
                    Year = reader.GetInt32(3),
                    LicensePlate = reader.GetString(4),
                    Status = Enum.Parse<VehicleStatus>(reader.GetString(5)),
                    CreatedAt = reader.GetDateTime(6),
                    UpdatedAt = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7)
                });
            }
        }

        return await Task.FromResult(vehicles);
    }
    
    /// <inheritdoc />
    public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
    {
        var reader = databaseService.QueryRow(GetAllVehiclesByIdQuery, vehicleId);

        if (reader.IsDBNull(0))
        {
            throw new InvalidOperationException("Vehicle not found");
        }
        
        return await Task.FromResult(new Vehicle
        {
            Id = reader.GetInt32(0),
            Make = reader.GetString(1),
            Model = reader.GetString(2),
            Year = reader.GetInt32(3),
            LicensePlate = reader.GetString(4),
            Status = Enum.Parse<VehicleStatus>(reader.GetString(5)),
            CreatedAt = reader.GetDateTime(6),
            UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7)
        });
    }
    
    /// <inheritdoc />
    public async Task<int> AddVehicleAsync(Vehicle vehicle)
    {
        return await Task.FromResult( databaseService.Execute(AddVehicleStmt, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.LicensePlate, vehicle.Status.ToString(), vehicle.CreatedAt));
    }
    
    /// <inheritdoc />
    public async Task<int> UpdateVehicleAsync(Vehicle vehicle)
    {
        return await Task.FromResult(databaseService.Execute(UpdateVehicleStmt, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.LicensePlate,vehicle.Status.ToString(), DateTime.UtcNow, vehicle.Id));
    }
    
    /// <inheritdoc />
    public async Task<int> DeleteVehicleAsync(int vehicleId)
    {
        return await Task.FromResult(databaseService.Execute(DeleteVehicleStmt, vehicleId));
    }
}
