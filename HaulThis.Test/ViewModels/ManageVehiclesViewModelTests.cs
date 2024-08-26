using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;
using System.Collections.ObjectModel;

namespace HaulThis.Tests.ViewModels
{
    public class ManageVehiclesViewModelTests
    {
        private readonly Mock<IManageVehiclesService> _mockManageVehiclesService;
        private readonly ManageVehiclesViewModel _viewModel;

        public ManageVehiclesViewModelTests()
        {
            _mockManageVehiclesService = new Mock<IManageVehiclesService>();
            _viewModel = new ManageVehiclesViewModel(_mockManageVehiclesService.Object);
        }

        [Fact]
        public async Task LoadVehicles_PopulatesVehiclesCollection()
        {
            // Arrange
            var mockVehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, Make = "Austin", Model = "Mini", Year = 1980, LicensePlate = "ABC123", CreatedAt = System.DateTime.UtcNow },
                new Vehicle { Id = 2, Make = "Porsche", Model = "Boxster", Year = 2019, LicensePlate = "XYZ789", CreatedAt = System.DateTime.UtcNow }
            };

            _mockManageVehiclesService.Setup(service => service.GetAllVehiclesAsync())
                .ReturnsAsync(mockVehicles);

            // Act
            await _viewModel.LoadVehicles();

            // Assert
            Assert.Equal(2, _viewModel.Vehicles.Count);
            Assert.Equal("Austin", _viewModel.Vehicles[0].Make);
            Assert.Equal("Porsche", _viewModel.Vehicles[1].Make);
        }

        [Fact]
        public async Task LoadVehicles_ClearsExistingVehicles()
        {
            // Arrange
            var initialVehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, Make = "Ford", Model = "Focus", Year = 2018, LicensePlate = "LMN456", CreatedAt = System.DateTime.UtcNow }
            };
            var newVehicles = new List<Vehicle>
            {
                new Vehicle { Id = 2, Make = "Austin", Model = "Healey", Year = 1980, LicensePlate = "JKL012", CreatedAt = System.DateTime.UtcNow }
            };

            _mockManageVehiclesService.Setup(service => service.GetAllVehiclesAsync())
                .ReturnsAsync(newVehicles);

            // Simulate initial state
            foreach (var vehicle in initialVehicles)
            {
                _viewModel.Vehicles.Add(vehicle);
            }

            // Act
            await _viewModel.LoadVehicles();

            // Assert
            Assert.Equal(1, _viewModel.Vehicles.Count);
            Assert.Equal("Austin", _viewModel.Vehicles[0].Make);
        }

        [Fact]
        public void OnPropertyChanged_RaisesPropertyChangedEvent()
        {
            // Arrange
            bool eventRaised = false;
            _viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.Vehicles))
                {
                    eventRaised = true;
                }
            };

            // Act
            _viewModel.OnPropertyChanged(nameof(_viewModel.Vehicles));

            // Assert
            Assert.True(eventRaised);
        }
    }
}
