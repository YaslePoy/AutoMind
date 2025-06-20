using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using AutoMind;
using PhysLab.DB;

namespace PhysLab.Pages;

public partial class MarketplacePage : Page
{
    private CalculatingEnvironment _currentEnviroment;
    private readonly Solution _solutions;
    MarketplaceViewModel vm = new();
    List<EnvironmentPack> _defaultPacks;

    public MarketplacePage(CalculatingEnvironment enviroment, Solution solutions)
    {
        _currentEnviroment = enviroment;
        _solutions = solutions;
        InitializeComponent();
        vm.VisiblePacks = PhysContext.Instance.EnvironmentPacks.Take(10).ToList();
        _defaultPacks = vm.VisiblePacks;
        DataContext = vm;
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var tempEnviroment = new CalculatingEnvironment(new DbPackProvider());
        var selected = ((sender as ListBox).SelectedValue as EnvironmentPack);
        if (selected != null)
        {
            tempEnviroment.AddRawEnvironmentPack(selected.Data);
        }

        vm.Selected = tempEnviroment;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(vm.SearchText))
        {
            vm.VisiblePacks = _defaultPacks;
            return;
        }

        vm.VisiblePacks = PhysContext.Instance.EnvironmentPacks.Where(i => i.Name == vm.SearchText).ToList();
    }

    private void AddPack(object sender, RoutedEventArgs e)
    {
        var pack = ((sender as Button).DataContext as CalculatingEnvironment).ImportPacks.First();
        _currentEnviroment.AddEnvironmentPack(pack.Identifier);
        PhysContext.Instance.ConnectedPacks.Add(new ConnectedPacks
        {
            Solution = _solutions,
            EnvironmentPackId = PhysContext.Instance.EnvironmentPacks
                .FirstOrDefault(i => i.Identifier == pack.Identifier).Id
        });
        PhysContext.Instance.SaveChanges();
    }

    private void GoBack(object sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }

    private void CreatePack(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new PackPublicationPage());
    }

    private void EditPack(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new PackPublicationPage((sender as MenuItem).DataContext as EnvironmentPack));
    }
}

public class MarketplaceViewModel : INotifyPropertyChanged
{
    private List<EnvironmentPack> _visiblePacks;
    private string _searchText;
    private CalculatingEnvironment _selected;

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

    public CalculatingEnvironment Selected
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