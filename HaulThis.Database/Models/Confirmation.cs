using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HaulThis.Database.Models;


[Table("Confirmations")]
[PrimaryKey("ConfirmationID")]
public class Confirmation
{
    
    public int ConfirmationID { get; set; }
    public Item Item { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; } // "Pickup" or "Delivery"
    public string ReceiverSignature { get; set; }

    // Constructor
    public Confirmation() { }

    // Additional methods and logic related to Confirmation can be added here
}

