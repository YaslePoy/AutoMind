using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using AutoMind;
using PhysLab.DB;

namespace PhysLab.Pages;

public partial class PackPublicationPage : Page
{
    private PubViewModel viewModel = new();
    private bool _isEditing = false;
    private EnvironmentPack CurrentPack;

    public PackPublicationPage()
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public PackPublicationPage(EnvironmentPack pack)
    {
        InitializeComponent();
        _isEditing = true;
        CurrentPack = pack;
        viewModel.Data = pack.Data;
        viewModel.Author = pack.Creator;
        viewModel.Name = pack.Name;
        viewModel.Id = pack.Identifier;
        DataContext = viewModel;
    }

    private void GoBack(object sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }

    private void Check(object sender, RoutedEventArgs e)
    {
        if (Check())
        {
            MessageBox.Show("Пакет не содержит ошибок");
            return;
        }

        MessageBox.Show("Пакет не может быть обработан. Исправьте ошибки!");
    }

    public bool Check()
    {
        try
        {
            var env = new CalculatingEnvironment(new DbPackProvider());
            env.AddRawEnvironmentPack(viewModel.Data);
            viewModel.Id = env.ImportPacks.First().Identifier;
            viewModel.EnablePublication = true;
            return true;
        }
        catch
        {
            viewModel.EnablePublication = false;
            return false;
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (!Check())
        {
            MessageBox.Show("Пакет не содержит ошибок");
            return;
        }

        if (!_isEditing)
        {
            if (PhysContext.Instance.EnvironmentPacks.Any(i => i.Identifier == viewModel.Id))
            {
                MessageBox.Show(
                    "Пакет с таким идентификатором уже существует. Дайте другой идентификатор во избежание конфликтов!");
                return;
            }

            var environmentPack = new EnvironmentPack
            {
                Data = viewModel.Data, Identifier = viewModel.Id, Creator = viewModel.Author, Name = viewModel.Name,
                CreatorId = PhysContext.User.Id
            };
            PhysContext.Instance.EnvironmentPacks.Add(environmentPack);
            PhysContext.Instance.SaveChanges();
            MessageBox.Show("Пакет успешно опубликован!!!");
            DataContext = null;
            DataContext = new PubViewModel();
        }
        else
        {
            if (viewModel.Id != CurrentPack.Identifier)
            {
                MessageBox.Show("Нельзя править идентификатор пакета!");
                return;
            }

            CurrentPack.Data = viewModel.Data;
            CurrentPack.Creator = viewModel.Author;
            CurrentPack.Name = viewModel.Name;
            PhysContext.Instance.EnvironmentPacks.Update(CurrentPack);
            PhysContext.Instance.SaveChanges();
        }
    }
}

public class PubViewModel : INotifyPropertyChanged
{
    private string _id;
    private bool _enablePublication;
    public string Author { get; set; }
    public string Name { get; set; }

    public bool EnablePublication
    {
        get => _enablePublication;
        set
        {
            if (value == _enablePublication) return;
            _enablePublication = value;
            OnPropertyChanged();
        }
    }

    public string Id
    {
        get => _id;
        set
        {
            if (value == _id) return;
            _id = value;
            OnPropertyChanged();
        }
    }

    public string Data { get; set; }
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