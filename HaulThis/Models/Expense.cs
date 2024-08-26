using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents an expense related to a trip in the haul-this system.
    /// </summary>
    public class Expense
    {
        /// <summary>
        /// Gets or sets the unique identifier for the expense.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the trip ID associated with the expense.
        /// </summary>
        [ForeignKey("Trip")]
        public int TripId { get; set; }

        /// <summary>
        /// Gets or sets the amount of the expense.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the description of the expense.
        /// </summary>
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of the expense.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Navigation property for the trip associated with the expense.
        /// </summary>
        public Trip Trip { get; set; }
    }
}
