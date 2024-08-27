using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models;

/// <summary>
/// Represents a delivery within a trip.
/// </summary>
public class Delivery : DataModel
{
    private readonly int _id;
    private int _tripId;
    private Waypoint _waypoint;
    private decimal _itemWeight;
    private string _customerName = string.Empty;
    private string _customerPhone = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for the delivery.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
        get => _id;
        init => SetProperty(ref _id, value);
    }
    
    /// <summary>
    /// Gets or sets the trip id associated with this delivery.
    /// </summary>
    [Required]
    public int TripId
    {
        get => _tripId;
        set => SetProperty(ref _tripId, value);
    }

    /// <summary>
    /// Gets or sets the waypoint associated with this delivery.
    /// </summary>
    [Required]
    public Waypoint Waypoint
    {
        get => _waypoint;
        set => SetProperty(ref _waypoint, value);
    }

    /// <summary>
    /// Gets or sets the weight of the item being delivered.
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal ItemWeight
    {
        get => _itemWeight;
        set => SetProperty(ref _itemWeight, value);
    }

    /// <summary>
    /// Gets or sets the name of the customer associated with this delivery.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string CustomerName
    {
        get => _customerName;
        set => SetProperty(ref _customerName, value);
    }

    /// <summary>
    /// Gets or sets the phone number of the customer associated with this delivery.
    /// </summary>
    [Required]
    [Phone]
    [MaxLength(15)]
    public string CustomerPhone
    {
        get => _customerPhone;
        set => SetProperty(ref _customerPhone, value);
    }
}