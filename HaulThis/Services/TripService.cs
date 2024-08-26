using HaulThis.Models;

namespace HaulThis.Services;

public class TripService(IDatabaseService databaseService) : ITripService
{
    private const string GetTripByDateQuery = """
                                         SELECT 
                                             t.Id AS TripId,
                                             v.vehicleName AS VehicleName,
                                             d.firstName + ' ' + d.lastName AS DriverName,
                                             w.location AS WaypointLocation,
                                             w.estimatedTime AS EstimatedTime,
                                         	i.Id AS itemId,
                                             i.itemWeight AS ItemWeight,
                                             u.firstName + ' ' + u.lastName AS CustomerName,
                                             u.phoneNumber AS CustomerPhone
                                         FROM 
                                             trip t
                                         INNER JOIN 
                                             vehicle v ON t.vehicleId = v.uniqueId
                                         INNER JOIN 
                                             users d ON t.driverId = d.Id
                                         INNER JOIN 
                                             item i ON i.tripId = t.Id
                                         INNER JOIN 
                                             bill b ON i.billId = b.Id
                                         INNER JOIN 
                                             users u ON b.userId = u.Id
                                         INNER JOIN 
                                             waypoint w ON w.tripId = t.Id
                                         WHERE 
                                             CAST(t.date AS DATE) = @p0
                                             AND i.delivered = 0
                                         ORDER BY 
                                            w.estimatedTime ASC;
                                         """;

    private const string MarkItemDeliveredStmt = """
                                             UPDATE item
                                             SET delivered = 1
                                             WHERE id = @p0 AND tripId = @p1;
                                             """;
    
    ///  <inheritdoc />
    public async Task<IEnumerable<Trip>> GetTripByDateAsync(DateTime date)
    {
        Dictionary<int, Trip> trips = new();

        using (var reader = databaseService.Query(GetTripByDateQuery, date.Date))
        {
            while (reader.Read())
            {
                int tripId = reader.GetInt32(0);

                if (!trips.TryGetValue(tripId, out Trip? trip))
                {
                    trip = new Trip
                    {
                        Id = tripId,
                        Vehicle = new Vehicle
                        {
                            VehicleName = reader.GetString(1)
                        },
                        Driver = new User
                        {
                            FirstName = reader.GetString(2)
                        },
                        TripManifest = []
                    };
                    
                    trips.Add(tripId, trip);
                }
                
                trip.TripManifest.Add(new Delivery
                {
                    Id = reader.GetInt32(5),
                    TripId = trip.Id,
                    ItemWeight = reader.GetDecimal(6),
                    CustomerName = reader.GetString(7),
                    CustomerPhone = reader.GetString(8),
                    Waypoint = new Waypoint
                    {
                        Location = reader.GetString(3),
                        EstimatedTime = reader.GetDateTime(4)
                    }
                });
            }
        }
        
        return await Task.FromResult(trips.Values);
    }

    public async Task<int> MarkItemAsDelivered(int tripId, int itemId)
    {
        return await Task.FromResult(databaseService.Execute(MarkItemDeliveredStmt, itemId, tripId));
    }
}