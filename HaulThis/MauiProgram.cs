using HaulThis.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using HaulThis.Views.Admin;
using Microsoft.Data.SqlClient;

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


        string connectionString = "Connection string";
        var loggerFactory = LoggerFactory.Create(loggerBuilder =>
        {
            loggerBuilder.AddConsole();
            loggerBuilder.AddDebug();
        });

        ILogger<DatabaseService> logger = loggerFactory.CreateLogger<DatabaseService>();
        logger.LogInformation("Attempting to connect");
        IDatabaseService db = new DatabaseService(new SqlConnection(connectionString), logger);
        logger.LogInformation("Connected successfully");
        logger.LogInformation("Attempting to ping");
        db.CreateConnection();
        if (db.Ping())
        {
            logger.LogInformation("pinged successfully");
        } else
        {
            logger.LogInformation("pinged unsuccessfully");
        }

        IUserService userService = new UserService(db);

        builder.Services.AddSingleton(db);
        builder.Services.AddSingleton(userService);
        builder.Services.AddTransient<ManageEmployees>(_ => new ManageEmployees(userService));

        return builder.Build();
    }
}