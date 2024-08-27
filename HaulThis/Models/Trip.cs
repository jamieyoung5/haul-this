using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models;

/// <summary>
/// Represents a trip in the system.
/// </summary>
public class Trip : DataModel
{
    private readonly int _id;
    private Vehicle _vehicle;
    private User _driver;
    private List<Delivery> _tripManifest = [];

    /// <summary>
    /// Gets or sets the unique identifier for the trip.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
        get => _id;
        init => SetProperty(ref _id, value);
    }

    /// <summary>
    /// Gets or sets the vehicle used for the trip.
    /// </summary>
    [Required]
    public Vehicle Vehicle
    {
        get => _vehicle;
        set => SetProperty(ref _vehicle, value);
    }

    /// <summary>
    /// Gets or sets the driver assigned to the trip.
    /// </summary>
    [Required]
    public User Driver
    {
        get => _driver;
        set => SetProperty(ref _driver, value);
    }

    /// <summary>
    /// Gets or sets the manifest of deliveries for the trip.
    /// </summary>
    [Required]
    public List<Delivery> TripManifest
    {
        get => _tripManifest;
        set => SetProperty(ref _tripManifest, value);
    }
}