using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents a manifest for a trip in the haul-this system
    /// </summary>
    public class TripManifest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the manifest.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the trip ID associated with the manifest.
        /// </summary>
        [ForeignKey("Trip")]
        public int TripId { get; set; }

        /// <summary>
        /// Gets or sets the item ID associated with the manifest.
        /// </summary>
        [ForeignKey("Item")]
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the pickup request ID associated with the manifest.
        /// </summary>
        [ForeignKey("PickupDeliveryRequest")]
        public int PickupRequestId { get; set; }

        /// <summary>
        /// Gets or sets the delivery request ID associated with the manifest.
        /// </summary>
        [ForeignKey("PickupDeliveryRequest")]
        public int DeliveryRequestId { get; set; }

        /// <summary>
        /// Navigation property for the trip associated with the manifest.
        /// </summary>
        public Trip Trip { get; set; }

        /// <summary>
        /// Navigation property for the item associated with the manifest.
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Navigation property for the pickup request associated with the manifest.
        /// </summary>
        public PickupDeliveryRequest PickupRequest { get; set; }

        /// <summary>
        /// Navigation property for the delivery request associated with the manifest.
        /// </summary>
        public PickupDeliveryRequest DeliveryRequest { get; set; }
    }
}