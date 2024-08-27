using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models;

/// <summary>
/// Represents a waypoint in a trip.
/// </summary>
public class Waypoint : DataModel
{
    private readonly int _id;
    private string _location = string.Empty;
    private DateTime _estimatedTime;

    /// <summary>
    /// Gets or sets the unique identifier for the waypoint.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
        get => _id;
        init => SetProperty(ref _id, value);
    }

    /// <summary>
    /// Gets or sets the location of the waypoint.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Location
    {
        get => _location;
        set => SetProperty(ref _location, value);
    }

    /// <summary>
    /// Gets or sets the estimated time of arrival at this waypoint.
    /// </summary>
    [Required]
    public DateTime EstimatedTime
    {
        get => _estimatedTime;
        set => SetProperty(ref _estimatedTime, value);
    }
}