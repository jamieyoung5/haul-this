using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Database.Models;



[Table("Item")]
[PrimaryKey("ItemID")]
public class Item
{
    public int ItemID { get; set; }
    public string Description { get; set; }
    public bool IsFragile { get; set; }
    public bool IsDangerous { get; set; }

    public ICollection<ItemLocation> ItemLocations { get; set; }

    public string TrackingID { get; set; }
    public string Status { get; set; }


    public Item()
    {
    }

    public Item(int itemID, string description, bool isFragile, bool isDangerous, string trackingID, string status)
    {
        ItemID = itemID;
        Description = description;
        IsFragile = isFragile;
        IsDangerous = isDangerous;
        TrackingID = trackingID;
        Status = status;
    }


}

