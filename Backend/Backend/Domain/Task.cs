namespace Backend.Domain;

public class Task
{
    public Task()
    {
    }

    public Task(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    public Task(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public Guid Id { get; init; }

    private string name;

    public string Name
    {
        get => name;
        init => name = value;
    }

    private string description;

    public string Description
    {
        get => description;
        init => description = value;
    }

    public void ReceivePropertiesFrom(Task toBeModified)
    {
        name = toBeModified.Name;
        description = toBeModified.Description;
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
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}