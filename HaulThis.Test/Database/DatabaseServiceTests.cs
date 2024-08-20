using Microsoft.Extensions.Logging;

public class DatabaseConnectionTests
{
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly Mock<ILogger<DatabaseService>> _mockLogger;
    private readonly DatabaseService _databaseConnection;

    public DatabaseConnectionTests()
    {
        _mockConnection = new Mock<IDbConnection>();
        _mockLogger = new Mock<ILogger<DatabaseService>>();
        _databaseConnection = new DatabaseService(_mockConnection.Object, _mockLogger.Object);
    }

    [Fact]
    public void CreateConnection_WhenConnectionIsClosed_ShouldOpenConnectionAndReturnTrue()
    {
        _mockConnection.Setup(c => c.State).Returns(ConnectionState.Closed);
        var result = _databaseConnection.CreateConnection();


        _mockConnection.Verify(c => c.Open(), Times.Once);
        _mockLogger.Verify(log => log.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Connected to the database successfully.")),
            null,
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
        ), Times.Once);
        Assert.True(result);


    }

    [Fact]
    public void CreateConnection_WhenConnectionIsOpen_ShouldNotOpenConnectionAndReturnTrue()
    {


        _mockConnection.Setup(c => c.State).Returns(ConnectionState.Open);
        var result = _databaseConnection.CreateConnection();

        _mockConnection.Verify(c => c.Open(), Times.Never);

        _mockLogger.Verify(log => log.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Connected to the database successfully.")),
            null,
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
        ), Times.Once);

        Assert.True(result);
    }

    [Fact]
    public void CreateConnection_WhenExceptionIsThrown_ShouldLogErrorAndReturnFalse()
    {
        _mockConnection.Setup(c => c.Open()).Throws(new Exception("Connection failed"));

        var result = _databaseConnection.CreateConnection();

        _mockLogger.Verify(log => log.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to connect to the database.")),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
        ), Times.Once);
        Assert.False(result);
    }
}

// Use the existing DatabaseService, cyclic dependency issue when trying to reference HaulThis.Services
public class DatabaseService
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
}
