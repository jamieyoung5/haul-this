using HaulThis.Models;

namespace HaulThis.Views.Misc;

public partial class ReportEmergencyOrDelayPopup
{
  public Report CreatedReport { get; private set; }

  public ReportEmergencyOrDelayPopup()
  {
    InitializeComponent();
  }

  private void OnCloseButtonClicked(object sender, EventArgs e)
  {
    Close();
  }

  private void OnSubmitButtonClicked(object sender, EventArgs e)
  {
    CreatedReport = new Report
    {
      TripId = int.Parse(TripIdEntry.Text),
      ReportType = ReportTypeEntry.Text,
      Description = DescriptionEntry.Text,
      ReportDateTime = DateTime.UtcNow
    };

    DialogResult = true;
    Close();
  }
}

