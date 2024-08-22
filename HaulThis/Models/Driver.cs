using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents a driver in the haul-this system
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// Gets or sets the unique identifier for the driver.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the driver.
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the qualifications of the driver.
        /// </summary>
        [MaxLength(255)]
        public string Qualifications { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for the associated user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Navigation property for trips assigned to the driver.
        /// </summary>
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
