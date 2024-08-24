using Xunit;
using Moq;
using System.Threading.Tasks;
using HaulThis.Models;
using HaulThis.Services;
using System.Collections.Generic;
using System.Data;

namespace HaulThis.Tests.Services
{
    public class ManageVehiclesServiceTests
    {
        private readonly Mock<IDatabaseService> _mockDatabaseService;
        private readonly ManageVehiclesService _service;

        public ManageVehiclesServiceTests()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _service = new ManageVehiclesService(_mockDatabaseService.Object);
        }

        [Fact]
        public async Task GetAllVehiclesAsync_ReturnsVehicleList()
        {
            // Arrange
            var mockDataReader = new Mock<IDataReader>();
            mockDataReader.SetupSequence(x => x.Read())
                .Returns(true)
                .Returns(false);
            mockDataReader.Setup(x => x.GetInt32(0)).Returns(1);
            mockDataReader.Setup(x => x.GetString(1)).Returns("Austin");
            mockDataReader.Setup(x => x.GetString(2)).Returns("Mini");
            mockDataReader.Setup(x => x.GetInt32(3)).Returns(1980);
            mockDataReader.Setup(x => x.GetString(4)).Returns("ABC123");
            mockDataReader.Setup(x => x.GetDateTime(5)).Returns(System.DateTime.UtcNow);
            mockDataReader.Setup(x => x.IsDBNull(6)).Returns(true);

            _mockDatabaseService.Setup(db => db.Query(It.IsAny<string>())).Returns(mockDataReader.Object);

            // Act
            var result = await _service.GetAllVehiclesAsync();

            // Assert
            var vehicleList = Assert.IsAssignableFrom<IEnumerable<Vehicle>>(result);
            Assert.Single(vehicleList);
            Assert.Equal("Austin", vehicleList.First().Make);
        }

        [Fact]
        public async Task GetVehicleByIdAsync_ReturnsVehicle()
        {
            // Arrange
            var mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(0)).Returns(false);
            mockDataReader.Setup(x => x.GetInt32(0)).Returns(1);
            mockDataReader.Setup(x => x.GetString(1)).Returns("Austin");
            mockDataReader.Setup(x => x.GetString(2)).Returns("Mini");
            mockDataReader.Setup(x => x.GetInt32(3)).Returns(1980);
            mockDataReader.Setup(x => x.GetString(4)).Returns("ABC123");
            mockDataReader.Setup(x => x.GetDateTime(5)).Returns(System.DateTime.UtcNow);
            mockDataReader.Setup(x => x.IsDBNull(6)).Returns(true);

            _mockDatabaseService.Setup(db => db.QueryRow(It.IsAny<string>(), It.IsAny<object[]>())).Returns(mockDataReader.Object);

            // Act
            var result = await _service.GetVehicleByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Austin", result.Make);
        }

        [Fact]
        public async Task AddVehicleAsync_ReturnsAffectedRows()
        {
            // Arrange
            var vehicle = new Vehicle { Make = "Austin", Model = "Mini", Year = 1980, LicensePlate = "ABC123", CreatedAt = System.DateTime.UtcNow };
            _mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);

            // Act
            var result = await _service.AddVehicleAsync(vehicle);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task UpdateVehicleAsync_ReturnsAffectedRows()
        {
            // Arrange
            var vehicle = new Vehicle { Id = 1, Make = "Austin", Model = "Mini", Year = 1980, LicensePlate = "ABC123", UpdatedAt = System.DateTime.UtcNow };
            _mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);

            // Act
            var result = await _service.UpdateVehicleAsync(vehicle);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task DeleteVehicleAsync_ReturnsAffectedRows()
        {
            // Arrange
            _mockDatabaseService.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);

            // Act
            var result = await _service.DeleteVehicleAsync(1);

            // Assert
            Assert.Equal(1, result);
        }
    }
}
