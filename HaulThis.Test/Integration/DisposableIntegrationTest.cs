using HaulThis.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace HaulThis.Test.Integration;

public class DisposableIntegrationTest : IDisposable
{
    protected readonly IDatabaseService _databaseService;
    private readonly SqlConnection _connection;
    private readonly DatabaseSetup _databaseSetup;
    private bool _disposed;

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
        if (_disposed) return;

        if (disposing)
            // Tear down the database before closing the connection
            _databaseSetup.TearDownDatabase(_connection);

        // Close the connection and dispose of it
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _databaseService.CloseConnection();
            _connection.Close();
            _connection.Dispose();
        }

        _disposed = true;
    }
}