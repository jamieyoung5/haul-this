using System.Data.Common;
using Microsoft.Extensions.Logging;
using HaulThis.Services;

namespace HaulThis.Tests.Services
{
    public class DatabaseServiceTests : IDisposable
    {
        private readonly Mock<IDbConnection> _mockDbConnection;
        private readonly Mock<ILogger<DatabaseService>> _mockLogger;
        private readonly DatabaseService _subject;

        public DatabaseServiceTests()
        {
            _mockDbConnection = new Mock<IDbConnection>();
            _mockLogger = new Mock<ILogger<DatabaseService>>();

            _subject = new DatabaseService(_mockDbConnection.Object, _mockLogger.Object);
        }

        [Fact]
        public void CreateConnection_WhenConnectionIsClosed_ShouldOpenConnectionAndReturnTrue()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Closed);

            // Act
            var result = _subject.CreateConnection();

            // Assert
            _mockDbConnection.Verify(c => c.Open(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public void CreateConnection_WhenConnectionIsAlreadyOpen_ShouldNotOpenConnectionAndReturnTrue()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Open);

            // Act
            var result = _subject.CreateConnection();

            // Assert
            _mockDbConnection.Verify(c => c.Open(), Times.Never);
            Assert.True(result);
        }

        [Fact]
        public void CreateConnection_WhenExceptionOccurs_ShouldReturnFalseAndLogError()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.Open()).Throws<InvalidOperationException>();

            // Act
            var result = _subject.CreateConnection();

            // Assert
            Assert.False(result);

            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Failed to connect to the database."),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void Ping_WhenConnectionIsOpen_ShouldReturnTrueAndLogInformation()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Open);

            var mockCommand = new Mock<IDbCommand>();
            mockCommand.Setup(c => c.ExecuteScalar()).Returns(1);

            _mockDbConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            var result = _subject.Ping();

            // Assert
            Assert.True(result);
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Pinged database successfully."),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void Ping_WhenConnectionIsClosed_ShouldReturnFalseAndLogError()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Closed);

            // Act
            var result = _subject.Ping();

            // Assert
            Assert.False(result);
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Connection is not open. Please call Connect first."),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void Execute_WhenConnectionIsOpen_ShouldReturnNumberOfRowsAffected()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Open);

            var mockCommand = new Mock<IDbCommand>();
            var mockParameter = new Mock<IDbDataParameter>();
            var mockParameterCollection = new Mock<IDataParameterCollection>();

            mockCommand.Setup(c => c.CreateParameter()).Returns(mockParameter.Object);
            mockCommand.SetupGet(c => c.Parameters).Returns(mockParameterCollection.Object);
            mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);

            _mockDbConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            var result = _subject.Execute("UPDATE SomeTable SET SomeColumn = @p0", "SomeValue");

            // Assert
            Assert.Equal(1, result);
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Executed command: UPDATE SomeTable SET SomeColumn = @p0"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void Execute_WhenCommandThrowsException_ShouldLogErrorAndThrowException()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Open);

            var mockCommand = new Mock<IDbCommand>();
            var mockParameter = new Mock<IDbDataParameter>();
            var mockParameterCollection = new Mock<IDataParameterCollection>();

            mockCommand.Setup(c => c.CreateParameter()).Returns(mockParameter.Object);
            mockCommand.SetupGet(c => c.Parameters).Returns(mockParameterCollection.Object);
            mockCommand.Setup(c => c.ExecuteNonQuery()).Throws<InvalidOperationException>();

            _mockDbConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            Assert.Throws<InvalidOperationException>(() => _subject.Execute("UPDATE SomeTable SET SomeColumn = @p0", "SomeValue"));

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Failed to execute command: UPDATE SomeTable SET SomeColumn = @p0"),
                    It.IsAny<InvalidOperationException>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void Query_WhenConnectionIsOpen_ShouldReturnDataReaderAndLogInformation()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Open);

            var mockCommand = new Mock<IDbCommand>();
            var mockReader = new Mock<DbDataReader>();
            mockCommand.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            _mockDbConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            var result = _subject.Query("SELECT * FROM SomeTable");

            // Assert
            Assert.NotNull(result);
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Executing query: SELECT * FROM SomeTable"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void QueryRow_WhenRowExists_ShouldReturnDataRecordAndLogInformation()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Open);

            var mockCommand = new Mock<IDbCommand>();
            var mockParameter = new Mock<IDbDataParameter>();
            var mockParameterCollection = new Mock<IDataParameterCollection>();
            var mockReader = new Mock<IDataReader>();

            mockCommand.Setup(c => c.CreateParameter()).Returns(mockParameter.Object);
            mockCommand.SetupGet(c => c.Parameters).Returns(mockParameterCollection.Object);
            mockReader.Setup(r => r.Read()).Returns(true);
            mockCommand.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            _mockDbConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            var result = _subject.QueryRow("SELECT * FROM SomeTable WHERE Id = @p0", 1);

            // Assert
            Assert.NotNull(result);
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Query returned a row: SELECT * FROM SomeTable WHERE Id = @p0"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }


        [Fact]
        public void QueryRow_WhenNoRowExists_ShouldReturnNullAndLogWarning()
        {
            // Arrange
            _mockDbConnection.Setup(c => c.State).Returns(ConnectionState.Open);

            var mockCommand = new Mock<IDbCommand>();
            var mockParameter = new Mock<IDbDataParameter>();
            var mockParameterCollection = new Mock<IDataParameterCollection>();
            var mockReader = new Mock<IDataReader>();

            mockCommand.Setup(c => c.CreateParameter()).Returns(mockParameter.Object);
            mockCommand.SetupGet(c => c.Parameters).Returns(mockParameterCollection.Object);
            mockReader.Setup(r => r.Read()).Returns(false);
            mockCommand.Setup(c => c.ExecuteReader()).Returns(mockReader.Object);

            _mockDbConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);

            // Act
            var result = _subject.QueryRow("SELECT * FROM SomeTable WHERE Id = @p0", 1);

            // Assert
            Assert.Null(result);
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Query did not return any rows: SELECT * FROM SomeTable WHERE Id = @p0"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}
