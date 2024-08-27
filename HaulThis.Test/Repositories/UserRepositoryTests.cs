using HaulThis.Models;
using HaulThis.Repositories;
using HaulThis.Services;

namespace HaulThis.Test.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<IDatabaseService> _mockDatabaseService;
    private readonly UserRepository _subject;

    public UserRepositoryTests()
    {
        _mockDatabaseService = new Mock<IDatabaseService>();
        _subject = new UserRepository(_mockDatabaseService.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenCalled_ShouldReturnAllUsers()
    {
        // Arrange
        Mock<IDataReader> mockReader = new Mock<IDataReader>();

        List<User> users = new List<User>
        {
            new()
            {
                Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890",
                Address = "123 Main St", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                Role = Role.Administrator
            },
            new()
            {
                Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", PhoneNumber = "0987654321",
                Address = "456 Elm St", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Role = Role.Customer
            }
        };

        SetupMockReader(mockReader, users);

        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>())).Returns(mockReader.Object);

        // Act
        IEnumerable<User> result = await _subject.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Count());

        for (int i = 0; i < users.Count; i++)
        {
            Assert.Equal(users[i].FirstName, result.ElementAt(i).FirstName);
            Assert.Equal(users[i].LastName, result.ElementAt(i).LastName);
            Assert.Equal(users[i].Email, result.ElementAt(i).Email);
            Assert.Equal(users[i].PhoneNumber, result.ElementAt(i).PhoneNumber);
            Assert.Equal(users[i].Address, result.ElementAt(i).Address);
            Assert.Equal(users[i].CreatedAt, result.ElementAt(i).CreatedAt);
            Assert.Equal(users[i].UpdatedAt, result.ElementAt(i).UpdatedAt);
            Assert.Equal(users[i].Role.ToString(), result.ElementAt(i).Role.ToString());
        }
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenNoUsersExist_ShouldReturnEmptyList()
    {
        // Arrange
        Mock<IDataReader> mockReader = new Mock<IDataReader>();
        mockReader.Setup(r => r.Read()).Returns(false); // No data
        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>())).Returns(mockReader.Object);

        // Act
        IEnumerable<User> result = await _subject.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllCustomersAsync_WhenCalled_ShouldReturnAllCustomers()
    {
        // Arrange
        Mock<IDataReader> mockReader = new Mock<IDataReader>();
        List<User> customers = new List<User>
        {
            new()
            {
                Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com",
                PhoneNumber = "1234567890", Address = "789 Pine St", CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow, Role = Role.Customer
            },
            new()
            {
                Id = 2, FirstName = "Bob", LastName = "Brown", Email = "bob@example.com", PhoneNumber = "0987654321",
                Address = "101 Maple St", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Role = Role.Customer
            }
        };

        SetupMockReader(mockReader, customers);
        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>())).Returns(mockReader.Object);

        // Act
        IEnumerable<User> result = await _subject.GetAllCustomersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customers.Count, result.Count());

        for (int i = 0; i < customers.Count; i++)
        {
            Assert.Equal(customers[i].FirstName, result.ElementAt(i).FirstName);
            Assert.Equal(customers[i].LastName, result.ElementAt(i).LastName);
            Assert.Equal(customers[i].Email, result.ElementAt(i).Email);
            Assert.Equal(customers[i].PhoneNumber, result.ElementAt(i).PhoneNumber);
            Assert.Equal(customers[i].Address, result.ElementAt(i).Address);
            Assert.Equal(customers[i].CreatedAt, result.ElementAt(i).CreatedAt);
            Assert.Equal(customers[i].UpdatedAt, result.ElementAt(i).UpdatedAt);
            Assert.Equal(customers[i].Role.ToString(), result.ElementAt(i).Role.ToString());
        }
    }

    [Fact]
    public async Task GetAllCustomersAsync_WhenNoCustomersExist_ShouldReturnEmptyList()
    {
        // Arrange
        Mock<IDataReader> mockReader = new Mock<IDataReader>();
        mockReader.Setup(r => r.Read()).Returns(false); // No data
        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>())).Returns(mockReader.Object);

        // Act
        IEnumerable<User> result = await _subject.GetAllCustomersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllEmployeesAsync_WhenCalled_ShouldReturnAllEmployees()
    {
        // Arrange
        Mock<IDataReader> mockReader = new Mock<IDataReader>();
        List<User> employees = new List<User>
        {
            new()
            {
                Id = 1, FirstName = "Charlie", LastName = "Johnson", Email = "charlie@example.com",
                PhoneNumber = "1234567890", Address = "202 Oak St", CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow, Role = Role.Administrator
            },
            new()
            {
                Id = 2, FirstName = "Dana", LastName = "Lee", Email = "dana@example.com", PhoneNumber = "0987654321",
                Address = "303 Birch St", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Role = Role.Driver
            }
        };

        SetupMockReader(mockReader, employees);
        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>())).Returns(mockReader.Object);

        // Act
        IEnumerable<User> result = await _subject.GetAllEmployeesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employees.Count, result.Count());

        for (int i = 0; i < employees.Count; i++)
        {
            Assert.Equal(employees[i].FirstName, result.ElementAt(i).FirstName);
            Assert.Equal(employees[i].LastName, result.ElementAt(i).LastName);
            Assert.Equal(employees[i].Email, result.ElementAt(i).Email);
            Assert.Equal(employees[i].PhoneNumber, result.ElementAt(i).PhoneNumber);
            Assert.Equal(employees[i].Address, result.ElementAt(i).Address);
            Assert.Equal(employees[i].CreatedAt, result.ElementAt(i).CreatedAt);
            Assert.Equal(employees[i].UpdatedAt, result.ElementAt(i).UpdatedAt);
            Assert.Equal(employees[i].Role.ToString(), result.ElementAt(i).Role.ToString());
        }
    }

    [Fact]
    public async Task GetAllEmployeesAsync_WhenNoEmployeesExist_ShouldReturnEmptyList()
    {
        // Arrange
        Mock<IDataReader> mockReader = new Mock<IDataReader>();
        mockReader.Setup(r => r.Read()).Returns(false); // No data
        _mockDatabaseService.Setup(ds => ds.Query(It.IsAny<string>())).Returns(mockReader.Object);

        // Act
        IEnumerable<User> result = await _subject.GetAllEmployeesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        Mock<IDataRecord> mockRecord = new Mock<IDataRecord>();

        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Role = Role.Administrator
        };

        SetupMockRecord(mockRecord, user);

        _mockDatabaseService.Setup(ds => ds.QueryRow(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns(mockRecord.Object);

        // Act
        var result = await _subject.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.PhoneNumber, result.PhoneNumber);
        Assert.Equal(user.Address, result.Address);
        Assert.Equal(user.CreatedAt, result.CreatedAt);
        Assert.Equal(user.UpdatedAt, result.UpdatedAt);
        Assert.Equal(user.Role.ToString(), result.Role.ToString());
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserDoesNotExist_ShouldThrowException()
    {
        // Arrange
        Mock<IDataRecord> mockRecord = new Mock<IDataRecord>();
        _mockDatabaseService.Setup(ds => ds.QueryRow(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns(mockRecord.Object);

        mockRecord.Setup(r => r.GetInt32(0)).Throws(new InvalidOperationException("User not found"));

        // Act / Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _subject.GetUserByIdAsync(999));
    }

    [Fact]
    public async Task AddUserAsync_WhenCalled_ShouldReturnNumberOfRowsAffected()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            Role = Role.Administrator,
            CreatedAt = DateTime.UtcNow
        };

        _mockDatabaseService.Setup(ds => ds.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);

        // Act
        int result = await _subject.AddUserAsync(user);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenCalled_ShouldReturnNumberOfRowsAffected()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            Role = Role.Administrator
        };

        _mockDatabaseService.Setup(ds => ds.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);

        // Act
        int result = await _subject.UpdateUserAsync(user);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenCalled_ShouldReturnNumberOfRowsAffected()
    {
        // Arrange
        _mockDatabaseService.Setup(ds => ds.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);

        // Act
        int result = await _subject.DeleteUserAsync(1);

        // Assert
        Assert.Equal(1, result);
    }

    private void SetupMockRecord(Mock<IDataRecord> mockRecord, User user)
    {
        mockRecord.Setup(r => r.GetInt32(0)).Returns(user.Id);
        mockRecord.Setup(r => r.GetString(1)).Returns(user.FirstName);
        mockRecord.Setup(r => r.GetString(2)).Returns(user.LastName);
        mockRecord.Setup(r => r.GetString(3)).Returns(user.Email);
        mockRecord.Setup(r => r.GetString(4)).Returns(user.PhoneNumber);
        mockRecord.Setup(r => r.GetString(5)).Returns(user.Address);
        mockRecord.Setup(r => r.GetDateTime(6)).Returns(user.CreatedAt);
        mockRecord.Setup(r => r.IsDBNull(7)).Returns(user.UpdatedAt == null);
        mockRecord.Setup(r => r.GetDateTime(7)).Returns(user.UpdatedAt ?? DateTime.MinValue);
        mockRecord.Setup(r => r.GetString(8)).Returns(user.Role.ToString());
    }

    private static void SetupMockReader(Mock<IDataReader> mockReader, List<User> users)
    {
        int currentIndex = -1;
        mockReader.Setup(r => r.Read()).Returns(() =>
        {
            currentIndex++;
            return currentIndex < users.Count;
        });

        mockReader.Setup(r => r.GetInt32(0)).Returns(() => users[currentIndex].Id);
        mockReader.Setup(r => r.GetString(1)).Returns(() => users[currentIndex].FirstName);
        mockReader.Setup(r => r.GetString(2)).Returns(() => users[currentIndex].LastName);
        mockReader.Setup(r => r.GetString(3)).Returns(() => users[currentIndex].Email);
        mockReader.Setup(r => r.GetString(4)).Returns(() => users[currentIndex].PhoneNumber);
        mockReader.Setup(r => r.GetString(5)).Returns(() => users[currentIndex].Address);
        mockReader.Setup(r => r.GetDateTime(6)).Returns(() => users[currentIndex].CreatedAt);
        mockReader.Setup(r => r.IsDBNull(7)).Returns(() => users[currentIndex].UpdatedAt == null);
        mockReader.Setup(r => r.GetDateTime(7)).Returns(() => users[currentIndex].UpdatedAt ?? DateTime.MinValue);
        mockReader.Setup(r => r.GetString(8)).Returns(() => users[currentIndex].Role.ToString());
    }
}