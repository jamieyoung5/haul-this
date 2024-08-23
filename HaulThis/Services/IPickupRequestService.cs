using System.Threading.Tasks;
using HaulThis.Models;

namespace HaulThis.Services
{
  public interface IPickupRequestService
  {
    Task<bool> RequestPickup(PickupRequest pickupRequest);
  }
}
