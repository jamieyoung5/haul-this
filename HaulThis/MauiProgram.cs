using HaulThis.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        string connectionString = "connection string";
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });

        ILogger<DatabaseService> _logger = loggerFactory.CreateLogger<DatabaseService>();
        _logger.LogInformation("Attempting to connect");
        IDatabaseService db = new DatabaseService(connectionString, _logger);
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
        

        return builder.Build();
    }
}