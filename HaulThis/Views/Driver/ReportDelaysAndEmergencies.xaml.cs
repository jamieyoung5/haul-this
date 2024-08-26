using CommunityToolkit.Maui.Views;
using HaulThis.Models;
using HaulThis.Services;
using HaulThis.ViewModels;
using HaulThis.Views.Misc;

namespace HaulThis.Views.Driver
{
  public partial class ReportDelaysAndEmergencies : ContentPage
  {
    private readonly IReportEmergencyService _reportEmergencyService;

    public ReportDelaysAndEmergencies(IReportEmergencyService reportEmergencyService)
    {
      InitializeComponent();
      _reportEmergencyService = reportEmergencyService ?? throw new ArgumentNullException(nameof(reportEmergencyService));
      BindingContext = new ManageReportsViewModel(_reportEmergencyService);
    }

    private async void OnLoadTripsButtonClicked(object sender, EventArgs e)
    {
      var viewModel = BindingContext as ManageReportsViewModel;
      if (viewModel != null)
      {
        await viewModel.LoadTripsFromDriver();
        TripPicker.ItemsSource = viewModel.Trips.Select(t => $"Trip ID: {t.Id}");
      }
    }

    private async void OnTripSelected(object sender, EventArgs e)
    {
      if (TripPicker.SelectedIndex != -1)
      {
        var selectedTrip = ((ManageReportsViewModel)BindingContext).Trips[TripPicker.SelectedIndex];
        ((ManageReportsViewModel)BindingContext).SelectedTrip = selectedTrip;
        await ((ManageReportsViewModel)BindingContext).LoadTripReports();
      }
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
      var reportPopup = new ReportEmergencyOrDelayPopup();
      var result = await this.ShowPopupAsync(reportPopup);

      if (result is not Report newReport) return;

      await _reportEmergencyService.AddReportAsync(newReport);

      // Refresh the view to include the new report
      var viewModel = BindingContext as ManageReportsViewModel;
      if (viewModel != null)
      {
        await viewModel.RefreshReports();
      }
    }

    private async void OnRefreshButtonClicked(object sender, EventArgs e)
    {
      var viewModel = BindingContext as ManageReportsViewModel;
      if (viewModel != null)
      {
        await viewModel.RefreshReports();
      }
    }
  }
}
