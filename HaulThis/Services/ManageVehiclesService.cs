using HaulThis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaulThis.Services;

public class ManageVehiclesService(IDatabaseService databaseService) : IManageVehiclesService
{
    private const string GetAllVehiclesQuery = "SELECT v.Id, v.make, v.model, v.year, v.licensePlate, v.createdAt, v.updatedAt FROM vehicles v";
    private const string GetAllVehiclesByIdQuery = "SELECT v.Id, v.make, v.model, v.year, v.licensePlate, v.createdAt, v.updatedAt FROM vehicles v WHERE v.Id = @p0";

    private const string AddVehicleStmt = """
                                        INSERT INTO vehicles (make, model, year, licensePlate, createdAt)
                                                              VALUES (@p0, @p1, @p2, @p3, @p4)
                                        """;
    private const string UpdateVehicleStmt = """
                                           UPDATE vehicles 
                                                                 SET make = @p0, model = @p1, year = @p2, licensePlate = @p3, updatedAt = @p4
                                                                 WHERE Id = @p5
                                           """;
    private const string DeleteVehicleStmt = "DELETE FROM vehicles WHERE id = @p0";

    public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
    {
        var vehicles = new List<Vehicle>();

        using (var reader = databaseService.Query(GetAllVehiclesQuery))
        {
            while (reader.Read())
            {
                vehicles.Add(new Vehicle
                {
                    Id = reader.GetInt32(0),
                    Make = reader.GetString(1),
                    Model = reader.GetString(2),
                    Year = reader.GetInt32(3),
                    LicensePlate = reader.GetString(4),
                    CreatedAt = reader.GetDateTime(5),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
                });
            }
        }

        return await Task.FromResult(vehicles);
    }

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
            CreatedAt = reader.GetDateTime(5),
            UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
        });
    }

    public async Task<int> AddVehicleAsync(Vehicle vehicle)
    {
        return await Task.FromResult( databaseService.Execute(AddVehicleStmt, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.LicensePlate, vehicle.CreatedAt));
    }

    public async Task<int> UpdateVehicleAsync(Vehicle vehicle)
    {
        return await Task.FromResult(databaseService.Execute(UpdateVehicleStmt, vehicle.Make, vehicle.Model, vehicle.Year, vehicle.LicensePlate, vehicle.UpdatedAt, vehicle.Id));
    }

    public async Task<int> DeleteVehicleAsync(int vehicleId)
    {
        return await Task.FromResult(databaseService.Execute(DeleteVehicleStmt, vehicleId));
    }


}
