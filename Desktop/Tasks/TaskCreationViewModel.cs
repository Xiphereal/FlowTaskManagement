using CommunityToolkit.Mvvm.Input;
using Desktop.Common;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public class TaskCreationViewModel : ViewModelBase
{
    public TaskCreationViewModel(IEnumerable<ITaskRepository> taskRepositories)
    {
        SaveTask =
            new AsyncRelayCommand<(ICloseable, string, string)>(async args =>
            {
                var (closeable, taskName, taskDescription) = args;

                CreatedTask = new Task(taskName, taskDescription);

                foreach (var repository in taskRepositories)
                {
                    await repository.Save(CreatedTask);
                }

                closeable!.Close();
            });
    }

    public IRelayCommand SaveTask { get; }

    public Task? CreatedTask { get; private set; }
}