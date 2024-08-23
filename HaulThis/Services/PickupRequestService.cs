using System;
using System.Threading.Tasks;
using HaulThis.Models;
using Microsoft.Extensions.Logging;

namespace HaulThis.Services
{
  public class PickupRequestService : IPickupRequestService
  {
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<PickupRequestService> _logger;

    private const string InsertPickupRequestQuery = @"
            INSERT INTO PickupRequests (PickupLocation, Destination, RequestedTime, CustomerName, CustomerContact, Status)
            VALUES (@PickupLocation, @Destination, @RequestedTime, @CustomerName, @CustomerContact, @Status)";

    public PickupRequestService(IDatabaseService databaseService)
    {
      _databaseService = databaseService;

      using var loggerFactory = LoggerFactory.Create(builder =>
      {
        builder.AddConsole();
        builder.AddDebug();
      });
      _logger = loggerFactory.CreateLogger<PickupRequestService>();
    }

    public async Task<bool> RequestPickup(PickupRequest pickupRequest)
    {
      try
      {
        _logger.LogInformation("Starting RequestPickup for PickupRequest: {PickupRequest}", pickupRequest);

        var parameters = new
        {
          pickupRequest.PickupLocation,
          pickupRequest.Destination,
          pickupRequest.RequestedTime,
          pickupRequest.CustomerName,
          pickupRequest.CustomerContact,
          pickupRequest.Status
        };

        int rowsAffected = await Task.Run(() => _databaseService.Execute(InsertPickupRequestQuery, parameters));

        if (rowsAffected > 0)
        {
          _logger.LogInformation("Pickup request inserted successfully for PickupRequest: {PickupRequest}", pickupRequest);
          return true;
        }
        else
        {
          _logger.LogWarning("No rows affected while inserting pickup request for PickupRequest: {PickupRequest}", pickupRequest);
          return false;
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred while inserting pickup request for PickupRequest: {PickupRequest}", pickupRequest);
        return false;
      }
    }
  }
}
