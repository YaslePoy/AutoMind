using System.Text.Json;
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
    private SolutionData _solutionData;

    public SolutionPage(Solution solution)
    {
        InitializeComponent();
        _current = solution;
        _solutionData = string.IsNullOrWhiteSpace(_current.InnerData)
            ? null
            : JsonSerializer.Deserialize<SolutionData>(_current.InnerData);
        if (_solutionData == null)
        {
            _solutionData = new SolutionData();
            _current.InnerData = JsonSerializer.Serialize(_solutionData);
            PhysContext.Instance.SaveChanges();
        }

        _solutionData.Solution = _current;
        _currentEnvironment = new CalculatingEnvironment(new DbPackProvider());
        int lastIndex = 0;
        foreach (var pack in PhysContext.Instance.ConnectedPacks.Where(i => i.SolutionId == solution.Id).ToList())
        {
            _currentEnvironment.AddRawEnvironmentPack(pack.EnvironmentPack.Data);
        }

        _currentEnvironment.ToTreeViewItem().ForEach(i => PacksTree.Items.Add(i));

        foreach (var addedFormula in _solutionData.FormulaViews)
        {
            var linkedFormula = _currentEnvironment.Functions.Find(i => i.RawView == addedFormula).Clone() as Formula;
            _solutionData.Formulas.Add(linkedFormula);
            Calcs.Items.Add(linkedFormula);
        }

        foreach (var property in _solutionData.PropertyViews)
        {
            var prop = _currentEnvironment.Properties.Find(i => i.ToString() == property.Identifier).Clone();
            _solutionData.Properties.Add(prop);
            prop.Value = property.Value;
            Inputs.Items.Add(prop);
        }

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
            if ((e.OriginalSource as TextBlock).DataContext is not "Входные данные")
            {
                return;
            }
            var data = e.Data.GetData(typeof(Property)) as Property;

            var currentItem = sender as TreeViewItem;
            var clone = data.Clone();
            currentItem.Items.Add(clone);
            _solutionData.Properties.Add(clone);
            _solutionData.PropertyViews.Add(clone.ToPropertyView());
            _solutionData.SaveAsync();
        }
    }

    private void Calcs_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(Formula)))
        {
            var data = e.Data.GetData(typeof(Formula)) as Formula;

            var currentItem = (sender as TreeViewItem);
            var clone = data.Clone() as Formula;
            currentItem.Items.Add(clone);
            _solutionData.Formulas.Add(clone);
            _solutionData.FormulaViews.Add(clone.RawView);
            _solutionData.SaveAsync();
        }
    }

    private void OpenFormula(object sender, MouseButtonEventArgs e)
    {
        var dc = (sender as TextBlock).DataContext as Formula;
        FormulaFrame.Content = new FormulaControl(dc, _solutionData);
    }

    private void DragProperty(object sender, MouseButtonEventArgs e)
    {
        var sp = sender as StackPanel;
        // if (sp.Tag is "Inputs")
        // {
        //     return;
        // }

        DragDrop.DoDragDrop(sp, sp.DataContext, DragDropEffects.Link);
    }

    private void GoBack(object sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}