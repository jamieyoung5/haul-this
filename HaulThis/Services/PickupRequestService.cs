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

    private const string GetPickupDeliveryRequestQuery = @"
            SELECT 
                Id,
                CustomerId,
                PickupLocation,
                DeliveryLocation,
                RequestedPickupDate,
                RequestedDeliveryDate,
                Status
            FROM 
                PickupDeliveryRequests
            WHERE 
                Id = @p0";

    public PickupRequestService(IDatabaseService databaseService)
    {
      _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

      using var loggerFactory = LoggerFactory.Create(builder =>
      {
        builder.AddConsole();
        builder.AddDebug();
      });
      _logger = loggerFactory.CreateLogger<PickupRequestService>();
    }

    public async Task<PickupDeliveryRequest?> GetPickupRequestInfo(int id)
    {
      try
      {
        _logger.LogInformation("Starting GetPickupRequestInfo for ID: {Id}", id);

        using var reader = _databaseService.Query(GetPickupDeliveryRequestQuery, id);

        if (reader.Read())
        {
          _logger.LogInformation("Query returned data for Request ID: {Id}", id);

          var request = new PickupDeliveryRequest
          {
            Id = reader.GetInt32(0),
            CustomerId = reader.GetInt32(1),
            PickupLocation = reader.GetString(2),
            DeliveryLocation = reader.GetString(3),
            RequestedPickupDate = reader.GetDateTime(4),
            RequestedDeliveryDate = reader.GetDateTime(5),
            Status = reader.GetString(6)
          };

          return request;
        }
        else
        {
          _logger.LogWarning("Query did not return any rows for Request ID: {Id}", id);
          return null;
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred while retrieving pickup/delivery request for ID: {Id}", id);
        return null;
      }
    }
  }
}
