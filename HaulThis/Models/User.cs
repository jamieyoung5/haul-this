using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HaulThis.Models;

/// <summary>
/// Represents a user in the haul-this system
/// </summary>
public class User : INotifyPropertyChanged
{
    private int _id;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;
    private string _phoneNumber = string.Empty;
    private Role _role = Role.Customer;
    private string _address = string.Empty;
    private DateTime _createdAt = DateTime.UtcNow;
    private DateTime? _updatedAt;

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    /// <summary>
    /// Gets or sets the user's phone number.
    /// </summary>
    [Required]
    [Phone]
    [MaxLength(15)]
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    /// <summary>
    /// Gets or sets the user's role in the application.
    /// </summary>
    [Required]
    [EnumDataType(typeof(Role))]
    public Role Role
    {
        get => _role;
        set => SetProperty(ref _role, value);
    }

    /// <summary>
    /// Gets or sets the user's address.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    [Required]
    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    /// <summary>
    /// Gets or sets the date and time when the user was last updated.
    /// </summary>
    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set => SetProperty(ref _updatedAt, value);
    }

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

/// <summary>
/// Enum representing the possible roles of a user in the application.
/// </summary>
public enum Role
{
    Customer,
    Driver,
    Administrator
}