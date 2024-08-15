using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HaulThis.Database.Models;


[Table("Locations")]
[PrimaryKey("LocationID")]
public class Location
{
    public int LocationID { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string County { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    // Constructor
    public Location()
    {

    }
    public Location(int locationID, string address, string city, string county, string postalCode, string country)
    {
        LocationID = locationID;
        Address = address;
        City = city;
        County = county;
        PostalCode = postalCode;
        Country = country;
    }

    // Override ToString to display location information easily
    public override string ToString()
    {
        return $"{Address}, {City}, {County} {PostalCode}, {Country}";
    }
}

