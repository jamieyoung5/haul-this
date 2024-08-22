using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents an event in the haul-this system
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the trip ID associated with the event.
        /// </summary>
        [ForeignKey("Trip")]
        public int TripId { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        [MaxLength(255)]
        public string EventType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time the event occurred.
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// Gets or sets the description of the event.
        /// </summary>
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for the trip associated with the event.
        /// </summary>
        public Trip Trip { get; set; }
    }
}
