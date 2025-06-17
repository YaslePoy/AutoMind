using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AutoMind;

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
        foreach (var totalProperty in ViewModel.WorkingFormula.TotalProperties)
        {
            if (totalProperty == formula.Head)
            {
                ViewModel.HeadProperty = totalProperty;
                continue;
            }

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal, Children =
                {
                    new TextBlock { Text = totalProperty.ToString(), Margin = new Thickness(5) },
                    new TextBlock { Text = " : ", Margin = new Thickness(5) },
                    new TextBox { AllowDrop = true, IsReadOnly = true, MinWidth = 20, Margin = new Thickness(5) }
                }
            };

            (stackPanel.Children[2] as TextBox).SetBinding(TextBox.TextProperty, new Binding("Value"));

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
    }

    private void SetupProperty(object sender, DragEventArgs e)
    {
        var prop = e.Data.GetData(typeof(Property)) as Property;
        
        _solutionData.Commutations.Add(new PropertyCommutation{ FormulasIndex = _solutionData.Formulas.IndexOf(ViewModel.OriginalFormula), PropertyIndex = _solutionData.Properties.IndexOf(prop!)});
        _solutionData.SaveAsync();
        (((sender as Border).Child as StackPanel).Children[2] as TextBox).DataContext =
            prop;
    }
}

public class FormulaEditorViewModel : INotifyPropertyChanged
{
    private Formula _originalFormula;
    private Formula _workingFormula;
    private Property _headProperty;
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
}