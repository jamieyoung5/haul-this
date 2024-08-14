namespace HaulThis.ViewModels;

using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Data.SqlClient;

public class TrackItemViewModel : INotifyPropertyChanged
{
    private string _trackingID;
    private Item _trackedItem;
    private string _statusMessage;


    public string TrackingID
    {
        get => _trackingID;
        set
        {
            _trackingID = value;
            OnPropertyChanged();
        }
    }

    public Item TrackedItem
    {
        get => _trackedItem;
        set
        {
            _trackedItem = value;
            OnPropertyChanged();
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    private HaulThisDbContext _dbContext;
    public ICommand TrackItemCommand { get; }

    public TrackItemViewModel(HaulThisDbContext haulThisDbContext)
    {
        _dbContext = haulThisDbContext;
        TrackItemCommand = new AsyncRelayCommand(TrackItem);
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Command Methods
    private async Task TrackItem()
    {
        if (string.IsNullOrEmpty(TrackingID))
        {
            StatusMessage = "Please enter a valid tracking ID.";
            return;
        }

        try
        {
            // Simulate fetching the tracked item from a service
            TrackedItem = GetItemByTrackingID(TrackingID);
            if (TrackedItem != null)
            {
                StatusMessage = $"Item found: {TrackedItem.Description} - Current Status: {GetItemStatus(TrackedItem)}";
            }
            else
            {
                StatusMessage = "Item not found. Please check the tracking ID.";
            }
        }
        catch (SqlException sqlEx)
        {
            // Handle SQL-specific errors
            StatusMessage = $"Database error: {sqlEx.Message}";

        }
        catch (Exception ex)
        {
            // Handle any errors that may occur during the tracking process
            StatusMessage = $"Error tracking item: {ex.Message}";
        }
    }

    // Mock method to simulate fetching an item by tracking ID
    private Item? GetItemByTrackingID(string trackingID)
    {
        try
        {
            Item? trackedItem = _dbContext.Item.FirstOrDefault(i => i.TrackingID == trackingID);
            return trackedItem;
        }
        catch (SqlException sqlEx)
        {
            // Handle SQL-specific errors
            StatusMessage = $"Database error: {sqlEx.Message}";
            return null;
        }
        catch (Exception ex)
        {
            // Handle any other errors that may occur during the tracking process
            StatusMessage = $"Error tracking item: {ex.Message}";
            return null;
        }
    }

    private string GetItemStatus(Item item)
    {

        string ItemStatus = item.Status;
        return ItemStatus;
    }
}

