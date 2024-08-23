using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents a waypoint in the haul-this system
    /// </summary>
    public class Waypoint
    {
        /// <summary>
        /// Gets or sets the unique identifier for the waypoint.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the trip ID associated with the waypoint.
        /// </summary>
        [ForeignKey("Trip")]
        public int TripId { get; set; }

        /// <summary>
        /// Gets or sets the location of the waypoint.
        /// </summary>
        [MaxLength(255)]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the arrival time at the waypoint.
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Gets or sets the departure time from the waypoint.
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Navigation property for the trip associated with the waypoint.
        /// </summary>
        public Trip Trip { get; set; }
    }
}
