using System.Data;
using Microsoft.Extensions.Logging;

namespace HaulThis.Services;

/// <summary>
/// Provides a common service for connecting and performing basic operations with a database
/// </summary>
public class DatabaseService : IDatabaseService
{
    private readonly IDbConnection _connection;
    private readonly ILogger<DatabaseService> _logger;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseService"/> class with the specified connection string.
    /// </summary>
    /// <param name="connection">The sql connection object.</param>
    /// <param name="logger">Generic logger</param>
    public DatabaseService(IDbConnection connection, ILogger<DatabaseService> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public bool CreateConnection()
    {
        try
        {
            _logger.LogInformation("Attempting to open database connection.");
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

            _logger.LogInformation("Connected to the database successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to the database.");
            return false;
        }
    }

    /// <inheritdoc />
    public bool Ping()
    {
        try
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogError("Connection is not open. Please call Connect first.");
                return false;
            }

            // Simple query to ensure the connection is active
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT 1";
            command.ExecuteScalar();

            _logger.LogInformation("Pinged database successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to ping the database.");
            return false;
        }
    }

    /// <inheritdoc />
    public void CloseConnection()
    {
        if (_connection.State == ConnectionState.Closed) return;
        _connection.Close();
        _logger.LogInformation("Database connecton closed.");
    }

    /// <inheritdoc />
    public int Execute(string query, params object[] args)
    {
        try
        {
            using var command = CreateCommand(query, args);
            int result = command.ExecuteNonQuery();
            _logger.LogInformation("Executed command: {Query}", query);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute command: {Query}", query);
            throw;
        }

    }

    /// <inheritdoc />
    public IDataReader Query(string query, params object[] args)
    {
        try
        {
            using var command = CreateCommand(query, args);
            _logger.LogInformation("Executing query: {Query}", query);
            return command.ExecuteReader();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute query: {Query}", query);
            throw;
        }
    }

    /// <inheritdoc />
    public IDataRecord QueryRow(string query, params object[] args)
    {
        try
        {
            using var command = CreateCommand(query, args);
            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                _logger.LogInformation("Query returned a row: {Query}", query);
                return reader;
            }

            _logger.LogWarning("Query did not return any rows: {Query}", query);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute query: {Query}", query);
            throw;
        }
    }

    private IDbCommand CreateCommand(string query, params object[] args)
    {
        var command = _connection.CreateCommand();
        command.CommandText = query;

        for (int i = 0; i < args.Length; i++)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = $"@p{i}";
            parameter.Value = args[i];
            command.Parameters.Add(parameter);
        }

        return command;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _connection?.Dispose();
        }

        _disposed = true;
    }
    
    ~DatabaseService()
    {
        Dispose(false);
    }
}