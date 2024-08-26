using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
  /// <summary>
  /// Represents a customer in the system.
  /// </summary>
  public class Customer : DataModel
  {
    private int _id;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _address = string.Empty;
    private string _city = string.Empty;
    private string _postalCode = string.Empty;
    private string _country = string.Empty;
    private DateTime _createdAt = DateTime.UtcNow;
    private DateTime? _updatedAt;
    private bool _isActive = true;

    /// <summary>
    /// Gets or sets the unique identifier for the customer.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get => _id;
      init => SetProperty(ref _id, value);
    }

    /// <summary>
    /// Gets or sets the customer's first name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName
    {
      get => _firstName;
      set => SetProperty(ref _firstName, value);
    }

    /// <summary>
    /// Gets or sets the customer's last name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName
    {
      get => _lastName;
      set => SetProperty(ref _lastName, value);
    }

    /// <summary>
    /// Gets or sets the customer's email address.
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
    /// Gets or sets the customer's phone number.
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
    /// Gets or sets the customer's address.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Address
    {
      get => _address;
      set => SetProperty(ref _address, value);
    }

    /// <summary>
    /// Gets or sets the city where the customer is located.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string City
    {
      get => _city;
      set => SetProperty(ref _city, value);
    }

    /// <summary>
    /// Gets or sets the postal code for the customer's address.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string PostalCode
    {
      get => _postalCode;
      set => SetProperty(ref _postalCode, value);
    }

    /// <summary>
    /// Gets or sets the country where the customer resides.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Country
    {
      get => _country;
      set => SetProperty(ref _country, value);
    }

    /// <summary>
    /// Gets or sets the date and time when the customer was created in the system.
    /// </summary>
    [Required]
    public DateTime CreatedAt
    {
      get => _createdAt;
      set => SetProperty(ref _createdAt, value);
    }

    /// <summary>
    /// Gets or sets the date and time when the customer's details were last updated.
    /// </summary>
    public DateTime? UpdatedAt
    {
      get => _updatedAt;
      set => SetProperty(ref _updatedAt, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the customer is active.
    /// </summary>
    [Required]
    public bool IsActive
    {
      get => _isActive;
      set => SetProperty(ref _isActive, value);
    }
  }
}
