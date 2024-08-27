using HaulThis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaulThis.Services;

/// <summary>
/// Interface defining Vehicle CRUD operations.
/// </summary>
public interface IManageVehiclesService
{
    /// <summary>
        /// Retrieves all vehicles asynchronously.
        /// </summary>
        /// <returns>A collection of all vehicles.</returns>
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        
        /// <summary>
        /// Retrieves a specific vehicle by its ID asynchronously.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with the specified ID.</returns>
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        
        /// <summary>
        /// Adds a new vehicle asynchronously.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> AddVehicleAsync(Vehicle vehicle);
        
        /// <summary>
        /// Updates an existing vehicle asynchronously.
        /// </summary>
        /// <param name="vehicle">The vehicle to update.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> UpdateVehicleAsync(Vehicle vehicle);
        
        /// <summary>
        /// Deletes a vehicle by ID asynchronously.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to delete.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> DeleteVehicleAsync(int vehicleId);
}
