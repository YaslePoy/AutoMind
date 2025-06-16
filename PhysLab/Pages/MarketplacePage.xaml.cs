using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using PhysLab.DB;

namespace PhysLab.Pages;

public partial class MarketplacePage : Page
{
    private Solution _currentSolution;
    MarketplaceViewModel vm = new();
    public MarketplacePage(Solution solution)
    {
        _currentSolution = solution;
        InitializeComponent();
        vm.VisiblePacks = PhysContext.Instance.EnvironmentPacks.Take(10).ToList();
        DataContext = vm;
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        vm.Selected = (sender as ListBox).SelectedValue as EnvironmentPack;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        vm.VisiblePacks = PhysContext.Instance.EnvironmentPacks.Where(i => i.Name == vm.Selected.Name).ToList();
    }
}

public class MarketplaceViewModel : INotifyPropertyChanged
{
    private List<EnvironmentPack> _visiblePacks;
    private string _searchText;
    private EnvironmentPack _selected;

    public List<EnvironmentPack> VisiblePacks
    {
        get => _visiblePacks;
        set
        {
            if (Equals(value, _visiblePacks)) return;
            _visiblePacks = value;
            OnPropertyChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (value == _searchText) return;
            _searchText = value;
            OnPropertyChanged();
        }
    }

    public EnvironmentPack Selected
    {
        get => _selected;
        set
        {
            if (Equals(value, _selected)) return;
            _selected = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}