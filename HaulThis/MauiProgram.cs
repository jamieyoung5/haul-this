using System.Reflection;
using System.Text.Json;
using HaulThis.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using HaulThis.Repositories;
using HaulThis.Repository;
using HaulThis.Views.Admin;
using Microsoft.Data.SqlClient;
using HaulThis.Views.Customer;
using HaulThis.Views.Driver;

namespace HaulThis;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var config = LoadConfiguration();
        string connectionString = config.ConnectionStrings.DevelopmentConnection;
        var loggerFactory = LoggerFactory.Create(loggerBuilder =>
        {
            loggerBuilder.AddConsole();
            loggerBuilder.AddDebug();
        });

        ILogger<DatabaseService> dbLogger = loggerFactory.CreateLogger<DatabaseService>();
        dbLogger.LogInformation("Attempting to connect");
        IDatabaseService db = new DatabaseService(
            new SqlConnection(connectionString), 
            dbLogger
            );
        dbLogger.LogInformation("Connected successfully");
        
        db.CreateConnection();
        dbLogger.LogInformation(db.Ping() ? "pinged successfully" : "pinged unsuccessfully");


        IUserRepository userRepository = new UserRepository(db);
        ITripRepository tripRepository = new TripRepository(db);
        IBillingRepository billingRepository = new BillingRepository(db);
        IItemRepository itemRepository = new ItemRepository(db);
        
        ITrackingService trackingService = new TrackingService(db);
        IPickupRequestService pickupRequestService = new PickupRequestService(db, loggerFactory.CreateLogger<PickupRequestService>());
        IManageVehiclesService manageVehiclesService = new ManageVehiclesService(db);
        IReportEmergencyService reportEmergencyService = new ReportEmergencyService(db);

        builder.Services.AddSingleton(pickupRequestService);
        builder.Services.AddSingleton(trackingService);
        builder.Services.AddTransient<TrackItem>(_ => new TrackItem(trackingService));
        builder.Services.AddTransient<RequestPickup>(_ => new RequestPickup(pickupRequestService));
        builder.Services.AddSingleton(db);
        builder.Services.AddSingleton(userRepository);
        builder.Services.AddTransient<ManageEmployees>(_ => new ManageEmployees(userRepository));
        builder.Services.AddTransient<ManageCustomers>(_ => new ManageCustomers(userRepository));
        builder.Services.AddSingleton(tripRepository);
        builder.Services.AddTransient<ManageTrips>(_ => new ManageTrips(tripRepository, itemRepository));  
        builder.Services.AddSingleton(manageVehiclesService);
        builder.Services.AddTransient<ManageVehicles>(_ => new ManageVehicles(manageVehiclesService));
        builder.Services.AddSingleton(billingRepository);
        builder.Services.AddTransient<ManageBilling>(_ => new ManageBilling(billingRepository));
        builder.Services.AddTransient<ReportDelaysAndEmergencies>(_ => new ReportDelaysAndEmergencies(reportEmergencyService));

        return builder.Build();
    }
    
    private static AppSettings LoadConfiguration()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream("HaulThis.appsettings.json");
        using StreamReader reader = new StreamReader(stream);
        string json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<AppSettings>(json);
    }
}