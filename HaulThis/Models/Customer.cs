using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models
{
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

    [Required]
    [MaxLength(50)]
    public string FirstName
    {
      get => _firstName;
      set => SetProperty(ref _firstName, value);
    }

    [Required]
    [MaxLength(50)]
    public string LastName
    {
      get => _lastName;
      set => SetProperty(ref _lastName, value);
    }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email
    {
      get => _email;
      set => SetProperty(ref _email, value);
    }

    [Required]
    [Phone]
    [MaxLength(15)]
    public string PhoneNumber
    {
      get => _phoneNumber;
      set => SetProperty(ref _phoneNumber, value);
    }

    [Required]
    [MaxLength(200)]
    public string Address
    {
      get => _address;
      set => SetProperty(ref _address, value);
    }

    [Required]
    [MaxLength(100)]
    public string City
    {
      get => _city;
      set => SetProperty(ref _city, value);
    }

    [Required]
    [MaxLength(20)]
    public string PostalCode
    {
      get => _postalCode;
      set => SetProperty(ref _postalCode, value);
    }

    [Required]
    [MaxLength(100)]
    public string Country
    {
      get => _country;
      set => SetProperty(ref _country, value);
    }

    [Required]
    public DateTime CreatedAt
    {
      get => _createdAt;
      set => SetProperty(ref _createdAt, value);
    }

    public DateTime? UpdatedAt
    {
      get => _updatedAt;
      set => SetProperty(ref _updatedAt, value);
    }

    [Required]
    public bool IsActive
    {
      get => _isActive;
      set => SetProperty(ref _isActive, value);
    }
  }
}
