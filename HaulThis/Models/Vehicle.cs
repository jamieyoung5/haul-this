using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HaulThis.Models
{
    /// <summary>
    /// Represents a vehicle in the haul-this system
    /// </summary>
    public class Vehicle
    {

        private int _id;
        private string _make = string.Empty;
        private string _model = string.Empty;
        private int _year;
        private string _licensePlate = string.Empty;
        private VehicleStatus _status = VehicleStatus.Available;
        private DateTime _createdAt = DateTime.UtcNow;
        private DateTime? _updatedAt;

        /// <summary>
        /// Gets or sets the unique identifier for the vehicle.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// Gets or sets the make of the vehicle.
        /// </summary>
        [MaxLength(255)]
        public string Make
        {
            get => _make;
            set => SetProperty(ref _make, value);
        }

        /// <summary>
        /// Gets or sets the model of the vehicle.
        /// </summary>
        [MaxLength(255)]
        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        /// <summary>
        /// Gets or sets the year of the vehicle.
        /// </summary>
        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        /// <summary>
        /// Gets or sets the license plate of the vehicle.
        /// </summary>
        [MaxLength(255)]
        public string LicensePlate
        {
            get => _licensePlate;
            set => SetProperty(ref _licensePlate, value);
        }

        /// <summary>
        /// Gets or sets the status of the vehicle.
        /// </summary>
        [MaxLength(255)]
        public VehicleStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }


        /// <summary>
        /// Gets or sets the date and time when the Vehicle was created.
        /// </summary>
        [Required]
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        /// <summary>
        /// Gets or sets the date and time when the Vehicle was last updated.
        /// </summary>
        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set => SetProperty(ref _updatedAt, value);
        }

        /// <summary>
        /// Navigation property for trips assigned to this vehicle.
        /// </summary>
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

   
    }
    public enum VehicleStatus
    {
        Available,       
        InUse,           
        InMaintenance,   
        Decommissioned   
    }
}