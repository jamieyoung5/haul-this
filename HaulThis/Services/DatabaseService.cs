using System;
using System.Data;
using Microsoft.Extensions.Logging;

namespace HaulThis.Services
{
    public class DatabaseService : IDatabaseService, IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<DatabaseService> _logger;

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

        public bool Ping()
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _logger.LogError("Connection is not open. Please call Connect first.");
                    return false;
                }

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

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Closed) return;
            _connection.Close();
            _logger.LogInformation("Database connection closed.");
        }

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

        public IDataReader Query(string query, params object[] args)
        {
            try
            {
                var command = CreateCommand(query, args); // Remove 'using' here
                _logger.LogInformation("Executing query: {Query}", query);
                return command.ExecuteReader(CommandBehavior.CloseConnection); // Ensure connection is closed when reader is closed
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute query: {Query}", query);
                throw;
            }
        }

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
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
