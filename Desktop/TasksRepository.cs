namespace Desktop;

using Task = Domain.Task;

public class TasksRepository : ITaskRepository
{
    public IReadOnlyList<Task> All()
    {
        return
        [
            new Task("A task", "Dummy description"),
            new Task("Another task", "Dummy description"),
            new Task("Yet another task", "Dummy description"),
        ];
    }
}