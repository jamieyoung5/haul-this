using HaulThis.Repositories;
using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Views.Customer;

public partial class ManageBilling
{
	private readonly IBillingRepository _billingRepository;
	
	public ManageBilling(IBillingRepository billingRepository)
	{
		InitializeComponent();
		_billingRepository = billingRepository ?? throw new ArgumentNullException(nameof(billingRepository));
		BindingContext = new BillingListViewModel(_billingRepository);
	}
}