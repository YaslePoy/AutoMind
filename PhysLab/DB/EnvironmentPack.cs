using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows;
using AutoMind;

namespace PhysLab.DB;

public class EnvironmentPack : DbNamed, INotifyPropertyChanged
{
    private Pack? _pack;
    public string Data { get; set; }
    public string Creator { get; set; }

    [ForeignKey("User")]
    public int CreatorId { get; set; }
    public virtual User User { get; set; }

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
    public string Identifier { get; set; }
    [NotMapped]
    public Visibility EditVisibility => CreatorId == PhysContext.User.Id ? Visibility.Visible : Visibility.Collapsed;
}