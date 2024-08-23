﻿using Microsoft.Data.SqlClient;

namespace HaulThis.Test.Integration;

public class DatabaseSetup
{
    public void InitializeDatabase(SqlConnection connection)
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
                    firstName NVARCHAR(50) NOT NULL,
                    lastName NVARCHAR(50) NOT NULL,
                    email NVARCHAR(100) NOT NULL UNIQUE,
                    phoneNumber NVARCHAR(15) NOT NULL,
                    address NVARCHAR(200) NOT NULL,
                    createdAt DATETIME NOT NULL,
                    updatedAt DATETIME NULL,
                    FOREIGN KEY (roleId) REFERENCES role(Id)
                );

                CREATE TABLE expense (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    userId INT,
                    amount DECIMAL(10, 2) NOT NULL,
                    FOREIGN KEY (userId) REFERENCES users(Id)
                );

                CREATE TABLE bill (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    userId INT,
                    FOREIGN KEY (userId) REFERENCES users(Id)
                );

                CREATE TABLE vehicle (
                    uniqueId INT PRIMARY KEY IDENTITY(1,1),
                    vehicleName VARCHAR(255) NOT NULL
                );

                CREATE TABLE trip (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    vehicleId INT,
                    driverId INT,
                    FOREIGN KEY (vehicleId) REFERENCES vehicle(uniqueId),
                    FOREIGN KEY (driverId) REFERENCES users(Id)
                );

                CREATE TABLE item (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    tripId INT,
                    billId INT,
                    pickedUpBy INT,
                    deliveredBy INT,
                    FOREIGN KEY (tripId) REFERENCES trip(Id),
                    FOREIGN KEY (billId) REFERENCES bill(Id),
                    FOREIGN KEY (pickedUpBy) REFERENCES users(Id),
                    FOREIGN KEY (deliveredBy) REFERENCES users(Id)
                );

                CREATE TABLE event (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    tripId INT,
                    eventName VARCHAR(255) NOT NULL,
                    FOREIGN KEY (tripId) REFERENCES trip(Id)
                );

                CREATE TABLE waypoint (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    tripId INT,
                    userId INT,
                    location VARCHAR(255) NOT NULL,
                    FOREIGN KEY (tripId) REFERENCES trip(Id),
                    FOREIGN KEY (userId) REFERENCES users(Id)
                );
            ";
            command.ExecuteNonQuery();
        }
    }

    public void ApplyMigrationChangeSet(SqlConnection connection)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                ALTER TABLE users 
                ADD anotherColumn NVARCHAR(50) NULL; -- Example modification
            ";
            command.ExecuteNonQuery();
        }
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

}