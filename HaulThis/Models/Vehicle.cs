using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents a vehicle in the haul-this system
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Gets or sets the unique identifier for the vehicle.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the make of the vehicle.
        /// </summary>
        [MaxLength(255)]
        public string Make { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the model of the vehicle.
        /// </summary>
        [MaxLength(255)]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the year of the vehicle.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the license plate of the vehicle.
        /// </summary>
        [MaxLength(255)]
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the vehicle.
        /// </summary>
        [MaxLength(255)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for trips assigned to this vehicle.
        /// </summary>
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
