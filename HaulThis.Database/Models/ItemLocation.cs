using System;

namespace HaulThis.Database.Models;

public class ItemLocation
{
    public int ItemLocationID { get; set; }
    public int ItemID { get; set; }
    public int LocationID { get; set; }
    public bool IsPickup { get; set; } // true for Pickup, false for Delivery
    
    public Item Item { get; set; }
    public Location Location { get; set; }
}
