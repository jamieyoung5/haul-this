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

    private const string GetPickupRequestQuery = @"
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

    private const string CreatePickupRequestQuery = @"
            INSERT INTO 
                PickupDeliveryRequests (CustomerId, PickupLocation, DeliveryLocation, RequestedPickupDate, RequestedDeliveryDate, Status)
            VALUES 
                (@p0, @p1, @p2, @p3, @p4, @p5);
            SELECT LAST_INSERT_ID();";

    public PickupRequestService(IDatabaseService databaseService, ILogger<PickupRequestService> logger)
    {
      _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <inheritdoc />
    public async Task<PickupDeliveryRequest?> GetPickupRequestInfo(int id)
    {
      try
      {
        _logger.LogInformation("Starting GetPickupRequestInfo for ID: {Id}", id);

        using var reader = _databaseService.Query(GetPickupRequestQuery, id);

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
    
    /// <inheritdoc />
    public async Task<int> CreatePickupRequest(PickupDeliveryRequest request)
    {
      try
      {
        _logger.LogInformation("Starting CreatePickupRequest for CustomerId: {CustomerId}", request.CustomerId);

        var result = _databaseService.Execute(CreatePickupRequestQuery,
            request.CustomerId,
            request.PickupLocation,
            request.DeliveryLocation,
            request.RequestedPickupDate,
            request.RequestedDeliveryDate,
            request.Status);

        _logger.LogInformation("Created pickup request with CustomerId: {CustomerId}", request.CustomerId);
        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred while creating pickup request for CustomerId: {CustomerId}", request.CustomerId);
        throw;
      }
    }
  }
}
