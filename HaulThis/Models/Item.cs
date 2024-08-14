namespace HaulThis;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Item")]
[PrimaryKey("ItemID")]
public class Item
{
    public int ItemID { get; set; }
    public string Description { get; set; }
    public bool IsFragile { get; set; }
    public bool IsDangerous { get; set; }
    public Location PickupLocation { get; set; }
    public Location DeliveryLocation { get; set; }
    public string TrackingID { get; set; }
    public string Status { get; set; }


     public Item()
    {
    }

    public Item(int itemID, string description, bool isFragile, bool isDangerous, Location pickupLocation, Location deliveryLocation,string trackingID, string status)
    {
        ItemID = itemID;
        Description = description;
        IsFragile = isFragile;
        IsDangerous = isDangerous;
        PickupLocation = pickupLocation;
        DeliveryLocation = deliveryLocation;
        TrackingID = trackingID;
        Status = status;
    }

    
}

