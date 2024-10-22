﻿using Microsoft.Data.SqlClient;

namespace HaulThis.Test.Integration;

public class DatabaseSetup
{
    public const string LocalDbString =
        "Server=(localdb)\\MSSQLLocalDB;Database=master;Integrated Security=true;MultipleActiveResultSets=true;";

    public SqlConnection DeployDatabase()
    {
        SqlConnection connection = null;
        int retryCount = 5;
        while (retryCount > 0)
            try
            {
                connection = new SqlConnection(LocalDbString);
                connection.Open();
                InitializeDatabase(connection);
                break;
            }
            catch (SqlException ex)
            {
                if (retryCount == 0 || !IsTransient(ex)) throw;
                retryCount--;
                Thread.Sleep(2000); // Wait for 2 seconds before retrying
            }

        return connection;
    }

    private bool IsTransient(SqlException ex)
    {
        // Handle common transient errors like SQL Server not ready
        return ex.Number == -2 || ex.Number == 4060 || ex.Number == 18456;
    }


    public void TearDownDatabase(SqlConnection connection)
    {
        // Switch to the master database before dropping the test db
        using (var switchCommand = new SqlCommand("USE master;", connection))
        {
            switchCommand.ExecuteNonQuery();
        }

        // Terminate all connections to the test db
        using (var terminateConnectionsCommand = new SqlCommand(
                   @"DECLARE @kill varchar(8000) = '';  
          SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
          FROM sys.dm_exec_sessions
          WHERE database_id = DB_ID('TestDb');
          EXEC(@kill);", connection))
        {
            terminateConnectionsCommand.ExecuteNonQuery();
        }

        // Drop the test database
        using (var dropCommand = new SqlCommand("DROP DATABASE IF EXISTS TestDb;", connection))
        {
            dropCommand.ExecuteNonQuery();
        }
    }

    private void InitializeDatabase(SqlConnection connection)
    {
        TearDownDatabase(connection);

        // Create testing database if it does not exist
        using (var command = new SqlCommand("IF DB_ID('TestDb') IS NULL CREATE DATABASE TestDb;", connection))
        {
            command.ExecuteNonQuery();
        }

        // Switch context to the new testing database
        using (var command = new SqlCommand("USE TestDb;", connection))
        {
            command.ExecuteNonQuery();
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                    CREATE TABLE role (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        roleName VARCHAR(255) NOT NULL
                    );

                    INSERT INTO role (roleName) VALUES ('Customer');
                    INSERT INTO role (roleName) VALUES ('Administrator');
                    INSERT INTO role (roleName) VALUES ('Driver');

                    CREATE TABLE users (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        roleId INT,
                        firstName NVARCHAR(50),
                        lastName NVARCHAR(50),
                        email NVARCHAR(100) UNIQUE,
                        phoneNumber NVARCHAR(15),
                        address NVARCHAR(200),
                        createdAt DATETIME,
                        updatedAt DATETIME NULL,
                        FOREIGN KEY (roleId) REFERENCES role(Id)
                    );

                    CREATE TABLE expense (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        userId INT,
                        amount DECIMAL(10, 2),
                        FOREIGN KEY (userId) REFERENCES users(Id)
                    );

                    CREATE TABLE bill (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        userId INT,
                        amount DECIMAL(10, 2) NOT NULL,
                        billDate DATETIME NOT NULL,
                        dueDate DATETIME NOT NULL,
                        status NVARCHAR(10) NOT NULL DEFAULT 'UNPAID',
                        FOREIGN KEY (userId) REFERENCES users(Id)
                    );

                    CREATE TABLE goodsCategory (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        categoryName NVARCHAR(255),
                        description NVARCHAR(255)
                    );

                    CREATE TABLE vehicle (
                        uniqueId INT PRIMARY KEY IDENTITY(1,1),
                        make NVARCHAR(255),
                        model NVARCHAR(255),
                        year INT,
                        licensePlate NVARCHAR(255),
                        status NVARCHAR(255) DEFAULT 'Available',
                        createdAt DATETIME,
                        updatedAt DATETIME NULL
                    );

                CREATE TABLE trip (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    vehicleId INT,
                    driverId INT,
                    date DATETIME NOT NULL,
                    FOREIGN KEY (vehicleId) REFERENCES vehicle(uniqueId),
                    FOREIGN KEY (driverId) REFERENCES users(Id)
                );

                CREATE TABLE item (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    tripId INT,
                    billId INT,
                    pickedUpBy INT,
                    deliveredBy INT,
                    itemWeight DECIMAL(10, 2),
                    delivered BIT NOT NULL,
                    FOREIGN KEY (tripId) REFERENCES trip(Id),
                    FOREIGN KEY (billId) REFERENCES bill(Id),
                    FOREIGN KEY (pickedUpBy) REFERENCES users(Id),
                    FOREIGN KEY (deliveredBy) REFERENCES users(Id)
                );

                    CREATE TABLE event (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        tripId INT,
                        eventName VARCHAR(255),
                        FOREIGN KEY (tripId) REFERENCES trip(Id)
                    );

                CREATE TABLE waypoint (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    tripId INT,
                    userId INT,
                    location VARCHAR(255) NOT NULL,
                    estimatedTime DATETIME,
                    FOREIGN KEY (tripId) REFERENCES trip(Id),
                    FOREIGN KEY (userId) REFERENCES users(Id)
                );
            ";
            command.ExecuteNonQuery();
        }
    }
}