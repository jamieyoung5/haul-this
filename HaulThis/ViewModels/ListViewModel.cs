using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HaulThis.ViewModels;

public class ListViewModel<TItem, TService> : INotifyPropertyChanged
{
    protected readonly TService _service;

    public ObservableCollection<TItem> Items { get; private set; } = [];

    public ListViewModel(TService service)
    {
        _service = service;
        LoadItems();
    }

    protected virtual async Task<IEnumerable<TItem>> GetItemsAsync()
    {
        // This method should be overridden by derived classes to provide specific data loading logic
        return new List<TItem>();
    }

    private async Task LoadItems()
    {
        var itemsFromDb = await GetItemsAsync();
        Items.Clear();

        foreach (var item in itemsFromDb)
        {
            Items.Add(item);
        }

        OnPropertyChanged(nameof(Items));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}