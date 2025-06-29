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

    public string Name { get; set; }
    public string Description { get; set; }

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