using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models;

/// <summary>
/// Represents a vehicle in the system.
/// </summary>
public class Vehicle : DataModel
{
    private readonly int _uniqueId;
    private string _vehicleName = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for the vehicle.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UniqueId
    {
        get => _uniqueId;
        init => SetProperty(ref _uniqueId, value);
    }

    /// <summary>
    /// Gets or sets the name of the vehicle.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string VehicleName
    {
        get => _vehicleName;
        set => SetProperty(ref _vehicleName, value);
    }
}