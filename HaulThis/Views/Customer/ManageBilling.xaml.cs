using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Views.Customer;

public partial class ManageBilling
{
	private readonly IBillingService _billingService;
	
	public ManageBilling(IBillingService billingService)
	{
		InitializeComponent();
		_billingService = billingService ?? throw new ArgumentNullException(nameof(billingService));
		var listService = new BillingListViewModel(_billingService);
		BindingContext = listService;
	}
}