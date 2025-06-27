namespace Backend.Domain;

public record Task
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

    public string Name { get; init; }
    public string Description { get; init; }
}