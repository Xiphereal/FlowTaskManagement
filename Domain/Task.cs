using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Desktop.Domain;

public class Task : INotifyPropertyChanged
{
    private string name;
    private string description;

    public string Name
    {
        get => name;
        set
        {
            if (value == name) 
                return;
            name = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => description;
        set
        {
            if (value == description) 
                return;
            description = value;
            OnPropertyChanged();
        }
    }

    public Task(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}