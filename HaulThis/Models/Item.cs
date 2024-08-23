using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents an item in the haul-this system
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the unique identifier for the item.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the weight of the item.
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets the category ID of the item.
        /// </summary>
        [ForeignKey("GoodsCategory")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Navigation property for the category of the item.
        /// </summary>
        public GoodsCategory Category { get; set; }

        /// <summary>
        /// Navigation property for the manifests that include this item.
        /// </summary>
        public ICollection<TripManifest> TripManifests { get; set; } = new List<TripManifest>();

        /// <summary>
        /// Navigation property for the inspection sign-offs for this item.
        /// </summary>
        public ICollection<InspectionSignOff> InspectionSignOffs { get; set; } = new List<InspectionSignOff>();
    }

  
}
