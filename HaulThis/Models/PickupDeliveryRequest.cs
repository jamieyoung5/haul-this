using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents a pickup or delivery request in the haul-this system
    /// </summary>
    public class PickupDeliveryRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the request.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the customer ID for the request.
        /// </summary>
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the pickup location for the request.
        /// </summary>
        [MaxLength(255)]
        public string PickupLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the delivery location for the request.
        /// </summary>
        [MaxLength(255)]
        public string DeliveryLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the requested pickup date for the request.
        /// </summary>
        public DateTime RequestedPickupDate { get; set; }

        /// <summary>
        /// Gets or sets the requested delivery date for the request.
        /// </summary>
        public DateTime RequestedDeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the status of the request.
        /// </summary>
        [MaxLength(255)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for the customer associated with the request.
        /// </summary>
        public Customer Customer { get; set; }
    }

   
}
