using HaulThis.Services;
using HaulThis.ViewModels;

namespace HaulThis.Views.Customer;

public partial class TrackItem : ContentPage
{
	private readonly ITrackingService _trackingService;
    public TrackItem(ITrackingService trackingService)
    {
        InitializeComponent();
		_trackingService = trackingService ?? throw new ArgumentNullException(nameof(trackingService));
		BindingContext = new TrackingViewModel(_trackingService);
    	
    }
}
