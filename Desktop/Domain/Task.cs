using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Desktop.Domain;

public class Task : INotifyPropertyChanged
{
    private string name;
    private string description;

    public Guid? Id { get; set; }

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

    public Task(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;

        return Equals((Task)obj);
    }

    private bool Equals(Task other)
    {
        return name == other.name && description == other.description;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name, description);
    }
}