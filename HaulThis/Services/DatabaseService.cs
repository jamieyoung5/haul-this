using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace HaulThis.Services
{
    /// <summary>
    /// Provides a common service for connecting and performing basic operations with a database
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private readonly DbConnection _connection;
        private readonly ILogger<DatabaseService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseService"/> class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        public DatabaseService(string connectionString, ILogger<DatabaseService> logger)
        {
            _connection = new SqlConnection(connectionString);
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
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _logger.LogInformation("Database connecton closed.");
            }
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
                else
                {
                    _logger.LogWarning("Query did not return any rows: {Query}", query);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute query: {Query}", query);
                throw;
            }
        }

        private DbCommand CreateCommand(string query, params object[] args)
        {
            var command = _connection.CreateCommand();
            command.CommandText = query;

            for (int i = 0; i < args.Length; i++)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = $"@p{i}";
                parameter.Value = args[i] ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }

            return command;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
