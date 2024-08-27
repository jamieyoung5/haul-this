namespace HaulThis;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        
		Routing.RegisterRoute(nameof(Views.Customer.ManageBilling), typeof(Views.Customer.ManageBilling));
		Routing.RegisterRoute(nameof(Views.Customer.RequestPickup), typeof(Views.Customer.RequestPickup));
		Routing.RegisterRoute(nameof(Views.Customer.TrackItem), typeof(Views.Customer.TrackItem));
		
		Routing.RegisterRoute(nameof(Views.Driver.ManageTrips), typeof(Views.Driver.ManageTrips));
		Routing.RegisterRoute(nameof(Views.Driver.RecordExpenses), typeof(Views.Driver.RecordExpenses));
		Routing.RegisterRoute(nameof(Views.Driver.ReportDelays), typeof(Views.Driver.ReportDelays));
	
		Routing.RegisterRoute(nameof(Views.Admin.ManageEmployees), typeof(Views.Admin.ManageEmployees));
		Routing.RegisterRoute(nameof(Views.Admin.ManageVehicles), typeof(Views.Admin.ManageVehicles));
		Routing.RegisterRoute(nameof(Views.Admin.ManageCustomers), typeof(Views.Admin.ManageCustomers));
    }
}