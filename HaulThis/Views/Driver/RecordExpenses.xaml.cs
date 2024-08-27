using CommunityToolkit.Maui.Views;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;
using HaulThis.Views.Misc;

namespace HaulThis.Views.Driver;

public partial class RecordExpenses : ContentPage
{	
	private readonly IManageExpensesService _manageExpensesService;
	public RecordExpenses(IManageExpensesService manageExpensesService)
	{
		InitializeComponent();
		_manageExpensesService = manageExpensesService ?? throw new ArgumentNullException(nameof(manageExpensesService));
		BindingContext = new ManageExpensesViewModel(_manageExpensesService);
	}

	private async void OnSelectTrip()
	{
		
	}

	private async void OnAddButtonClicked(object sender, EventArgs e)
	{
		var addExpensePopup = new AddExpensePopup();
		object? result = await this.ShowPopupAsync(addExpensePopup);

		if (result is not Expense newExpense) return;

		await _manageExpensesService.AddExpenseAsync(newExpense);

		BindingContext = new ManageExpensesViewModel(_manageExpensesService);
	}

	private void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		var button = sender as Button;

		if (button?.CommandParameter is not Expense expenseToDelete) return;
		_manageExpensesService.DeleteExpenseAsync(expenseToDelete.Id);

		var viewModel = BindingContext as ManageExpensesViewModel;

		viewModel?.Expenses.Remove(expenseToDelete);
	}
	private async void OnRefreshButtonClicked(object sender, EventArgs e)
	{
		var viewModel = BindingContext as ManageExpensesViewModel;
		if (viewModel != null)
		{
			await viewModel.RefreshExpenses();
		}
	}
}