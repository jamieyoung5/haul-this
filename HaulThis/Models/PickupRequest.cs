namespace HaulThis.Models
{
  public class PickupRequest
  {
    public string PickupLocation { get; set; }
    public string Destination { get; set; }
    public DateTime RequestedTime { get; set; }
    public string CustomerName { get; set; }
    public string CustomerContact { get; set; }
    public string Status { get; set; }
  }
}
