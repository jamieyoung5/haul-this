using System.Reflection;
using System.Text.Json;
using HaulThis.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using HaulThis.Views.Admin;
using Microsoft.Data.SqlClient;
using HaulThis.ViewModels;
using HaulThis.Views.Customer;

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
        if (db.Ping())
        {
            logger.LogInformation("pinged successfully");
        } else
        {
            logger.LogInformation("pinged unsuccessfully");
        }


        ITrackingService trackingService = new TrackingService(db);
        IUserService userService = new UserService(db);
        IManageVehiclesService manageVehiclesService = new ManageVehiclesService(db);
        
        builder.Services.AddSingleton(trackingService);
        builder.Services.AddTransient<TrackItem>(_ => new TrackItem(trackingService));
        builder.Services.AddSingleton(db);
        builder.Services.AddSingleton(userService);
        builder.Services.AddTransient<ManageEmployees>(_ => new ManageEmployees(userService));
        builder.Services.AddSingleton(manageVehiclesService);
        builder.Services.AddTransient<ManageVehicles>(_ => new ManageVehicles(manageVehiclesService));



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