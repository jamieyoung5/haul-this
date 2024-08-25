namespace HaulThis;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
    
		Routing.RegisterRoute(nameof(Views.Customer.ManageAccount), typeof(Views.Customer.ManageAccount));
		Routing.RegisterRoute(nameof(Views.Customer.ManageBilling), typeof(Views.Customer.ManageBilling));
		Routing.RegisterRoute(nameof(Views.Customer.RequestPickup), typeof(Views.Customer.RequestPickup));
		Routing.RegisterRoute(nameof(Views.Customer.TrackItem), typeof(Views.Customer.TrackItem));
		Routing.RegisterRoute(nameof(Views.Customer.CustomerConfirmPickup), typeof(Views.Customer.CustomerConfirmPickup));
		
		
		Routing.RegisterRoute(nameof(Views.Driver.ManageTrips), typeof(Views.Driver.ManageTrips));
		Routing.RegisterRoute(nameof(Views.Driver.RecordExpenses), typeof(Views.Driver.RecordExpenses));
		Routing.RegisterRoute(nameof(Views.Driver.DriverConfirmPickup), typeof(Views.Driver.DriverConfirmPickup));
		Routing.RegisterRoute(nameof(Views.Driver.ReportDelays), typeof(Views.Driver.ReportDelays));
	
	
		Routing.RegisterRoute(nameof(Views.Admin.ManageEmployees), typeof(Views.Admin.ManageEmployees));
		Routing.RegisterRoute(nameof(Views.Admin.ManageVehicles), typeof(Views.Admin.ManageVehicles));
		Routing.RegisterRoute(nameof(Views.Admin.ManageCustomers), typeof(Views.Admin.ManageCustomers));
		Routing.RegisterRoute(nameof(Views.Admin.ManageCustomerBills), typeof(Views.Admin.ManageCustomerBills));
		Routing.RegisterRoute(nameof(Views.Admin.ManageTripManifests), typeof(Views.Admin.ManageTripManifests));
		Routing.RegisterRoute(nameof(Views.Admin.PlanRouteTrips), typeof(Views.Admin.PlanRouteTrips));
		Routing.RegisterRoute(nameof(Views.Admin.ResourceTrips), typeof(Views.Admin.ResourceTrips));
		Routing.RegisterRoute(nameof(Views.Admin.TrackTrips), typeof(Views.Admin.TrackTrips));
    }
}