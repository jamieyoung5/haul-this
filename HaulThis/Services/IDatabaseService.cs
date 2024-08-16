using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaulThis.Services
{

    /// <summary>
    /// Interface for a database connection service
    /// </summary>
    public interface IDatabaseService : IDisposable
    {
        bool CreateConnection();

        /// <summary>
        /// Pings the database to check the connection.
        /// </summary>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        bool Ping();

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Executes a non-query SQL command.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="args">The parameters for the SQL query.</param>
        /// <returns>The number of rows affected.</returns>
        int Execute(string query, params object[] args);

        /// <summary>
        /// Executes a SQL query and returns multiple rows.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="args">The parameters for the SQL query.</param>
        /// <returns>A data reader to read the result set.</returns>
        IDataReader Query(string query, params object[] args);

        /// <summary>
        /// Executes a SQL query and returns a single row.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="args">The parameters for the SQL query.</param>
        /// <returns>The data record for the single row.</returns>
        IDataRecord QueryRow(string query, params object[] args);
    }
}
