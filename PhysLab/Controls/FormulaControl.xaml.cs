using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AutoMind;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Property = AutoMind.Property;

namespace PhysLab.Controls;

public partial class FormulaControl : UserControl
{
    FormulaEditorViewModel ViewModel = new FormulaEditorViewModel();
    SolutionData _solutionData;

    public FormulaControl(Formula formula, SolutionData solutionData)
    {
        InitializeComponent();
        _solutionData = solutionData;
        ViewModel.OriginalFormula = formula;
        ViewModel.WorkingFormula = formula;

        BuildForFormula(ViewModel.WorkingFormula);
        DataContext = ViewModel;
    }

    private void BuildForFormula(Formula formula)
    {
        HeadValue.DataContext = null;
        var canCalc = true;
        foreach (var totalProperty in ViewModel.WorkingFormula.TotalProperties)
        {
            if (totalProperty == formula.Head)
            {
                var headCurrentIndex =
                    ViewModel.OriginalFormula.TotalProperties.FindIndex(i => i.ToString() == totalProperty.ToString());
                int headFormulaIndex = _solutionData.Formulas.IndexOf(ViewModel.OriginalFormula);
                if (_solutionData.Commutations.Find(i =>
                        i.FormulaPropertyIndex == headCurrentIndex && i.FormulasIndex == headFormulaIndex) is
                    { } headCommutation)
                {
                    HeadValue.DataContext = _solutionData.Properties[headCommutation.PropertyIndex];
                }
                else
                {
                    canCalc = false;
                }

                ViewModel.HeadProperty = totalProperty;
                continue;
            }

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal, Children =
                {
                    new TextBlock
                        { Text = totalProperty.ToString(), Margin = new Thickness(5), DataContext = totalProperty },
                    new TextBlock { Text = " : ", Margin = new Thickness(5) },
                    new TextBox { AllowDrop = true, IsReadOnly = true, MinWidth = 20, Margin = new Thickness(5) }
                }
            };

            var viewChild = (stackPanel.Children[2] as TextBox);
            viewChild.SetBinding(TextBox.TextProperty, new Binding("Value"));

            var currentIndex =
                ViewModel.OriginalFormula.TotalProperties.FindIndex(i => i.ToString() == totalProperty.ToString());
            int formulaIndex = _solutionData.Formulas.IndexOf(ViewModel.OriginalFormula);
            if (_solutionData.Commutations.Find(i =>
                    i.FormulaPropertyIndex == currentIndex && i.FormulasIndex == formulaIndex) is { } commutation)
            {
                viewChild.DataContext = _solutionData.Properties[commutation.PropertyIndex];
            }
            else
            {
                canCalc = false;
            }

            var border = new Border
            {
                Child = stackPanel
            };

            border.Drop += SetupProperty;

            void OnStackPanelOnPreviewMouseDown(object s, MouseButtonEventArgs e)
            {
                BodyProperties.Children.Clear();
                ViewModel.WorkingFormula = formula.ExpressFrom(totalProperty);
                BuildForFormula(ViewModel.WorkingFormula);
            }

            stackPanel.PreviewMouseDown += OnStackPanelOnPreviewMouseDown;

            BodyProperties.Children.Add(border);
        }

        CalcButton.IsEnabled = canCalc;
    }

    private void SetupProperty(object sender, DragEventArgs e)
    {
        var prop = e.Data.GetData(typeof(Property)) as Property;

        var stackPanel = (sender as Border).Child as StackPanel;

        if (((stackPanel.Children[0] as TextBlock).DataContext as Property).UnitsFull != prop.UnitsFull)
        {
            return;
        }

        var propertyCommutation = new PropertyCommutation
        {
            FormulasIndex = _solutionData.Formulas.IndexOf(ViewModel.OriginalFormula),
            PropertyIndex = _solutionData.Properties.IndexOf(prop!),
            FormulaPropertyIndex = ViewModel.OriginalFormula.TotalProperties.FindIndex(i =>
                i.ToString() == (stackPanel.Children[0] as TextBlock).Text)
        };
        _solutionData.Commutations.RemoveAll(i =>
            i.FormulasIndex == propertyCommutation.FormulasIndex &&
            i.FormulaPropertyIndex == propertyCommutation.FormulaPropertyIndex);
        _solutionData.Commutations.Add(propertyCommutation);
        _solutionData.SaveAsync();
        (stackPanel.Children[2] as TextBox).DataContext =
            prop;
    }

    private void Calculate(object sender, RoutedEventArgs e)
    {
        var formulaIndex = _solutionData.Formulas.IndexOf(ViewModel.OriginalFormula);
        var commutatuions = _solutionData.Commutations.Where(i => i.FormulasIndex == formulaIndex).ToList();
        var originalFormulaTotalProperties = ViewModel.OriginalFormula.TotalProperties;
        if (commutatuions.Count != originalFormulaTotalProperties.Count)
        {
            return;
        }

        commutatuions = commutatuions.OrderBy(i => i.FormulaPropertyIndex).ToList();

        Property headLink = null;
        for (int i = 0; i < originalFormulaTotalProperties.Count; i++)
        {
            originalFormulaTotalProperties[i].Value = _solutionData.Properties[commutatuions[i].PropertyIndex].Value;
            if (ViewModel.WorkingFormula.Head == originalFormulaTotalProperties[i])
            {
                headLink = _solutionData.Properties[commutatuions[i].PropertyIndex];
            }
        }

        var sb = new StringBuilder();
        sb.Append(ViewModel.OriginalFormula);
        sb.AppendLine(" =>");
        if (ViewModel.WorkingFormula.Head != ViewModel.OriginalFormula.Head)
        {
            sb.Append(ViewModel.WorkingFormula);
            sb.AppendLine(" =>");
        }

        sb.Append(ViewModel.WorkingFormula.ToValue());
        sb.Append(" => ");
        var result = ViewModel.WorkingFormula.Expression.GetValue();
        // ViewModel.HeadProperty.Value = result;
        headLink.Value = result;


        sb.Append(headLink.ToValue());
        sb.Append(" ");
        sb.Append(headLink.UnitsShort);
        ViewModel.Solving = sb.ToString();
        ViewModel.OnPropertyChanged(headLink, "Value");
        _solutionData.SaveAsync();
    }
}

public class FormulaEditorViewModel : INotifyPropertyChanged
{
    private Formula _originalFormula;
    private Formula _workingFormula;
    private Property _headProperty;
    private string _solving;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnPropertyChanged(object sender, string? propertyName = null)
    {
        PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public Formula OriginalFormula
    {
        get => _originalFormula;
        set
        {
            if (Equals(value, _originalFormula)) return;
            _originalFormula = value;
            OnPropertyChanged();
        }
    }

    public Formula WorkingFormula
    {
        get => _workingFormula;
        set
        {
            if (Equals(value, _workingFormula)) return;
            _workingFormula = value;
            OnPropertyChanged();
        }
    }

    public Property HeadProperty
    {
        get => _headProperty;
        set
        {
            if (Equals(value, _headProperty)) return;
            _headProperty = value;
            OnPropertyChanged();
        }
    }

    public string Solving
    {
        get => _solving;
        set
        {
            if (value == _solving) return;
            _solving = value;
            OnPropertyChanged();
        }
    }
}