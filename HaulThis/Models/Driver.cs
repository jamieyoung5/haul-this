namespace HaulThis;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Drivers")]
[PrimaryKey("DriverID")]
public class Driver
{
    public int DriverID { get; set; }
    public string Name { get; set; }
    public bool IsQualifiedForDangerousGoods { get; set; }

    // Constructor
    public Driver()
    {
    }
    public Driver(int driverID, string name, bool isQualifiedForDangerousGoods)
    {
        DriverID = driverID;
        Name = name;
        IsQualifiedForDangerousGoods = isQualifiedForDangerousGoods;
    }

    // Other methods and logic related to Driver can be added here
}
