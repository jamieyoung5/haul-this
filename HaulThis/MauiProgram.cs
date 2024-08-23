using HaulThis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.Options;
using HaulThis.ViewModels;
using HaulThis.Views.Customer;
using Microsoft.Data.SqlClient;

namespace HaulThis;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var configBuilder = new ConfigurationBuilder();
        configBuilder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("HaulThis/appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = configBuilder.Build();

        string connectionString = config.GetConnectionString("DevelopmentConnection");
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });

        ILogger<DatabaseService> _logger = loggerFactory.CreateLogger<DatabaseService>();
        _logger.LogInformation("Attempting to connect");
        IDatabaseService db = new DatabaseService(new SqlConnection(connectionString), _logger);
        _logger.LogInformation("Connected successfully");
        _logger.LogInformation("Attempting to ping");
        db.CreateConnection();
        if (db.Ping())
        {
            _logger.LogInformation("pinged successfully");
        } else
        {
            _logger.LogInformation("pinged unsuccessfully");
        }

         // Register services and ViewModel
        ITrackingService trackingService = new TrackingService(db);
        builder.Services.AddSingleton(trackingService);

        IPickupRequestService pickupRequestService = new PickupRequestService(db);
        builder.Services.AddSingleton(pickupRequestService);

        // Register the pages
        builder.Services.AddTransient<TrackItem>(_ => new TrackItem(trackingService));
        builder.Services.AddTransient<RequestPickup>(_ => new RequestPickup(pickupRequestService));

        return builder.Build();
    }
}