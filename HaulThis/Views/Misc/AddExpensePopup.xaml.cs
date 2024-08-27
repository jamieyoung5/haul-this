using HaulThis.Models;

namespace HaulThis.Views.Misc;

public partial class AddExpensePopup 
{
	private Expense CreatedExpense { get; set; }

	public AddExpensePopup()
	{
		InitializeComponent();
	}

	private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        CreatedExpense = new Expense
		{
			TripId = int.Parse(TripIdEntry.Text),
			Date = DateTime.UtcNow,
			Amount = decimal.Parse(AmountEntry.Text),
			Description = DescriptionEntry.Text,
		};
        
        Close(CreatedExpense);
    }
}
