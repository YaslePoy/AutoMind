using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PhysLab.DB;

namespace PhysLab.Pages;

public partial class SolvingPage : Page
{
    private SolvingPageViewModel vm;

    public SolvingPage()
    {
        InitializeComponent();
        vm = new SolvingPageViewModel(PhysContext.Instance.Solutions.Where(i => i.UserId == PhysContext.User.Id)
            .ToList());
        DataContext = vm;
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        vm.Selected = (sender as ListBox).SelectedValue as Solution;
        DescView.Visibility = vm.Selected is null ? Visibility.Collapsed : Visibility.Visible;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(vm.SearchText) || vm.Solutions.Any(i => i.Name == vm.SearchText))
            return;

        PhysContext.Instance.Solutions.Add(new Solution { Name = vm.SearchText, UserId = PhysContext.User.Id, Description = string.Empty});
        PhysContext.Instance.SaveChanges();

        vm = new SolvingPageViewModel(PhysContext.Instance.Solutions.Where(i => i.UserId == PhysContext.User.Id)
            .ToList());
        
        DataContext = null;
        DataContext = vm;
    }
    private void ShowView(object sender, MouseButtonEventArgs e)
    {
        DescEdit.Visibility = Visibility.Collapsed;
        DescView.Visibility = Visibility.Visible;
    }

    private void ShowEditor(object sender, RoutedEventArgs e)
    {
        DescEdit.Visibility = Visibility.Visible;
        DescView.Visibility = Visibility.Collapsed;
    }

    private void Save(object sender, RoutedEventArgs e)
    {
        ShowView(null, null);
        PhysContext.Instance.SaveChanges();
    }

    private void Rollback(object sender, RoutedEventArgs e)
    {
        ShowView(null, null);

        vm.Selected.Description = PhysContext.Instance.Solutions.FirstOrDefault(i => i.Id == vm.Selected.Id)!.Description;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        vm.SearchText = (sender as TextBox).Text;
    }

    private void GotoSolution(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new SolutionPage(vm.Selected));
    }
}

public sealed class SolvingPageViewModel : INotifyPropertyChanged
{
    private List<Solution> _solutionsAll;
    private List<Solution> _solutions;
    private Solution _selected;
    private string _searchText;

    public List<Solution> Solutions
    {
        get => _solutions;
        set
        {
            if (value.SequenceEqual(_solutions)) return;
            _solutions = value;
            OnPropertyChanged();
        }
    }

    public Solution Selected
    {
        get => _selected;
        set
        {
            if (Equals(value, _selected)) return;
            _selected = value;
            OnPropertyChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            if (string.IsNullOrWhiteSpace(_searchText))
            {
                Solutions = _solutionsAll;
            }

            Solutions = _solutionsAll
                .Where(s => s.Name.Contains(_searchText, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public SolvingPageViewModel(List<Solution> solutions)
    {
        _solutionsAll = solutions;
        _solutions = new List<Solution>(_solutionsAll);
    }
}