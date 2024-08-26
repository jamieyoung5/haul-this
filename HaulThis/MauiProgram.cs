using System.Reflection;
using System.Text.Json;
using HaulThis.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
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

        ILogger<DatabaseService> logger = loggerFactory.CreateLogger<DatabaseService>();
        logger.LogInformation("Attempting to connect");
        IDatabaseService db = new DatabaseService(new SqlConnection(connectionString), logger);
        logger.LogInformation("Connected successfully");
        db.CreateConnection();
        logger.LogInformation(db.Ping() ? "pinged successfully" : "pinged unsuccessfully");


        ITrackingService trackingService = new TrackingService(db);
        IUserService userService = new UserService(db);
        ITripService tripService = new TripService(db);
        
        builder.Services.AddSingleton(trackingService);
        builder.Services.AddTransient<TrackItem>(_ => new TrackItem(trackingService));
        builder.Services.AddSingleton(db);
        builder.Services.AddSingleton(userService);
        builder.Services.AddTransient<ManageEmployees>(_ => new ManageEmployees(userService));
        builder.Services.AddSingleton(tripService);
        builder.Services.AddTransient<ManageTrips>(_ => new ManageTrips(tripService));
        
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