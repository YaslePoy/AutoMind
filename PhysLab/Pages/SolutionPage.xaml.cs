using System.Windows;
using System.Windows.Controls;
using PhysLab.DB;

namespace PhysLab.Pages;

public partial class SolutionPage : Page
{
    private Solution _current;
    public SolutionPage(Solution solution)
    {
        InitializeComponent();
        _current = solution;
        DataContext = _current;
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MarketplacePage(_current));
    }
}