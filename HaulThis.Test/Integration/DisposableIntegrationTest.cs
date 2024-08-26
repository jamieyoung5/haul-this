using HaulThis.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace HaulThis.Test.Integration;

public class DisposableIntegrationTest : IDisposable
{
    protected readonly IDatabaseService _databaseService;
    private readonly SqlConnection _connection;
    private readonly DatabaseSetup _databaseSetup;

    protected DisposableIntegrationTest()
    {
        _databaseSetup = new DatabaseSetup();
        _connection = _databaseSetup.DeployDatabase();
        var loggerFactory = LoggerFactory.Create(loggerBuilder =>
        {
            loggerBuilder.AddConsole();
            loggerBuilder.AddDebug();
        });
        ILogger<DatabaseService> logger = loggerFactory.CreateLogger<DatabaseService>();
        _databaseService = new DatabaseService(_connection, logger);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _databaseSetup.TearDownDatabase(_connection);
        _databaseService.CloseConnection(); 
    }
}