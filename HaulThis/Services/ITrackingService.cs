using System.Threading.Tasks;
using HaulThis.Models;

namespace HaulThis.Services
{
    public interface ITrackingService
    {
        /// <summary>
        /// Retrieves tracking information for the given tracking ID.
        /// </summary>
        /// <param name="trackingId">The ID of the item to track.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of <see cref="TrackingInfo"/> containing the tracking information.</returns>
        Task<TrackingInfo> GetTrackingInfo(string trackingId);
    }
}
