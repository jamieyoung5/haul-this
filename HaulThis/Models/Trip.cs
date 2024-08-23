using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents a trip in the haul-this system
    /// </summary>
    public class Trip
    {
        /// <summary>
        /// Gets or sets the unique identifier for the trip.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the vehicle ID associated with the trip.
        /// </summary>
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the driver ID associated with the trip.
        /// </summary>
        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        /// <summary>
        /// Gets or sets the start date of the trip.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the trip.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the status of the trip.
        /// </summary>
        [MaxLength(255)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for the vehicle associated with the trip.
        /// </summary>
        public Vehicle Vehicle { get; set; }

        /// <summary>
        /// Navigation property for the driver associated with the trip.
        /// </summary>
        public Driver Driver { get; set; }

        /// <summary>
        /// Navigation property for the manifests associated with the trip.
        /// </summary>
        public ICollection<TripManifest> TripManifests { get; set; } = new List<TripManifest>();

        /// <summary>
        /// Navigation property for the waypoints associated with the trip.
        /// </summary>
        public ICollection<Waypoint> Waypoints { get; set; } = new List<Waypoint>();

        /// <summary>
        /// Navigation property for the events associated with the trip.
        /// </summary>
        public ICollection<Event> Events { get; set; } = new List<Event>();

        /// <summary>
        /// Navigation property for the expenses associated with the trip.
        /// </summary>
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }

   
}
