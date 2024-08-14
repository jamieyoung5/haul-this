namespace HaulThis;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Trips")]
[PrimaryKey("TripID")]
public class Trip
{
    public int TripID { get; set; }
    public Driver Driver { get; set; }
    public string Vehicle { get; set; }
    public List<Item> Items { get; set; }
    public List<Location> Locations { get; set; }

    // Constructor
    public Trip()
    {
    }
    public Trip(int tripID, Driver driver, string vehicle)
    {
        TripID = tripID;
        Driver = driver;
        Vehicle = vehicle;
        Items = new List<Item>();
        Locations = new List<Location>();
    }

    // Method to add an item to the trip and track its locations
    public void AddItem(Item item)
    {
        Items.Add(item);

        // Add the item's pickup and delivery locations to the trip's location list if they aren't already present
        if (!Locations.Contains(item.PickupLocation))
        {
            Locations.Add(item.PickupLocation);
        }
        if (!Locations.Contains(item.DeliveryLocation))
        {
            Locations.Add(item.DeliveryLocation);
        }
    }

    // Method to confirm pickup/delivery of an item
    public Confirmation ConfirmPickupOrDelivery(Item item, string type, string receiverSignature = null)
    {
        // Logic to check if the driver is qualified to handle dangerous goods
        if (item.IsDangerous && !Driver.IsQualifiedForDangerousGoods)
        {
            throw new InvalidOperationException("Driver is not qualified to handle dangerous goods.");
        }

        // Logic to handle fragile items that require a signature
        if (item.IsFragile && string.IsNullOrEmpty(receiverSignature))
        {
            throw new InvalidOperationException("Receiver signature is required for fragile items.");
        }

        // Create a new confirmation record
        Confirmation confirmation = new Confirmation
        {
            ConfirmationID = new Random().Next(1, 100000), // This should be replaced with a proper ID generation strategy
            Item = item,
            Timestamp = DateTime.Now,
            Type = type,
            ReceiverSignature = receiverSignature
        };

        // Here you would typically save the confirmation to a database

        return confirmation;
    }

    // Other methods and logic related to Trip can be added here
}

