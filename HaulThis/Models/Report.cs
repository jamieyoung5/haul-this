using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
  /// <summary>
  /// Represents a delay or emergency report related to a trip in the haul-this system.
  /// </summary>
  public class Report
  {
    /// <summary>
    /// Gets or sets the unique identifier for the report.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the trip ID associated with the report.
    /// </summary>
    [ForeignKey("Trip")]
    public int TripId { get; set; }

    /// <summary>
    /// Gets or sets the type of the report (e.g., Delay, Emergency).
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the delay or emergency.
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the report was made.
    /// </summary>
    [Required]
    public DateTime ReportDateTime { get; set; }
  }
}
