using System.Threading.Tasks;
using HaulThis.Models;

namespace HaulThis.Services
{
  public interface IPickupRequestService
  {
    /// <summary>
    /// Creates a PickupDeliveryRequest and supplies the request.
    /// </summary>
    /// <param name="id">The unique identifier of the pickup/delivery request.</param>
    /// <returns>A task representing the asynchronous operation, containing the PickupDeliveryRequest object if found, otherwise null.</returns>
    Task<int> CreatePickupRequest(PickupDeliveryRequest request);
    /// <summary>
    /// Retrieves a PickupDeliveryRequest by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the pickup/delivery request.</param>
    /// <returns>A task representing the asynchronous operation, containing the PickupDeliveryRequest object if found, otherwise null.</returns>
    Task<PickupDeliveryRequest?> GetPickupRequestInfo(int id);
  }
}
