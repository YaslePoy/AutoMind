using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoMind;
using PhysLab.Controls;
using PhysLab.DB;

namespace PhysLab.Pages;

public partial class SolutionPage : Page
{
    private Solution _current;
    private CalculatingEnvironment _currentEnvironment;

    public SolutionPage(Solution solution)
    {
        InitializeComponent();
        _current = solution;
        _currentEnvironment = new CalculatingEnvironment(new DbPackProvider());
        int lastIndex = 0;
        foreach (var pack in PhysContext.Instance.ConnectedPacks.Where(i => i.SolutionId == solution.Id).ToList())
        {
            _currentEnvironment.AddRawEnvironmentPack(pack.EnvironmentPack.Data);
        }

        _currentEnvironment.ToTreeViewItem().ForEach(i => PacksTree.Items.Add(i));


        DataContext = _current;
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MarketplacePage(_currentEnvironment, _current));
    }

    private void Inputs_OnDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(Property)))
        {
            var data = e.Data.GetData(typeof(Property)) as Property;

            var currentItem = (sender as TreeViewItem);
            currentItem.Items.Add(data);
        }
    }

    private void Calcs_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(Formula)))
        {
            var data = e.Data.GetData(typeof(Formula)) as Formula;

            var currentItem = (sender as TreeViewItem);
            currentItem.Items.Add(data);
        }
    }

    private void OpenFormula(object sender, MouseButtonEventArgs e)
    {
        var dc = (sender as TextBlock).DataContext as Formula;
        FormulaFrame.Content = new FormulaControl(dc);
    }

    private void DragProperty(object sender, MouseButtonEventArgs e)
    {
        DragDrop.DoDragDrop(sender as StackPanel, (sender as StackPanel).DataContext, DragDropEffects.Link);
    }
}