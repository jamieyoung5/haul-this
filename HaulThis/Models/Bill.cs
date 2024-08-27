using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulThis.Models;

/// <summary>
/// Represents a bill that is to be paid by a user.
/// </summary>
public class Bill : DataModel
{
    private readonly int _id;
    private int userId;
    private decimal _amount;
    private DateTime _billDate;
    private DateTime _dueDate;
    private BillStatus _status;
    
    /// <summary>
    /// Gets or sets the unique identifier for the bill.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
        get => _id;
        init => SetProperty(ref _id, value);
    }

    /// <summary>
    /// Gets or sets the unique identifier for the user due to pay the bill.
    /// </summary>
    [ForeignKey("userId")]
    public int UserId
    {
        get => userId;
        init => SetProperty(ref userId, value);
    }

    /// <summary>
    /// Gets or sets the amount due in the bill.
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }

    /// <summary>
    /// Gets or sets the date the bill was issued
    /// </summary>
    [Required]
    public DateTime BillDate
    {
        get => _billDate;
        set => SetProperty(ref _billDate, value);
    }

    
    /// <summary>
    /// Gets or sets the date the bill is ude
    /// </summary>
    [Required]
    public DateTime DueDate
    {
        get => _dueDate;
        set => SetProperty(ref _dueDate, value);
    }

    /// <summary>
    /// Gets or sets the status of the bill, e.g paid, unpaid, overdue
    /// </summary>
    [Required]
    public BillStatus Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }
}

public enum BillStatus
{
    PAID,
    UNPAID,
    OVERDUE
}