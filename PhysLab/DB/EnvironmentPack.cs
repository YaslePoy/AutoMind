using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using AutoMind;

namespace PhysLab.DB;

public class EnvironmentPack : DbNamed, INotifyPropertyChanged
{
    private Pack? _pack;
    public string Data { get; set; }
    public string Creator { get; set; }
    [NotMapped]
    public string Mark { get; set; }

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

    [NotMapped]
    public Pack Pack
    {
        get
        {
            if (_pack is null)
            {
                
            }
            return _pack;
        }
    }

    public string Identifier { get; set; }
}