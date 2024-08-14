namespace HaulThis.Views.Customer;
using HaulThis.ViewModels;

public partial class TrackItem : ContentPage
{
	public TrackItem(TrackItemViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
        
	}
}