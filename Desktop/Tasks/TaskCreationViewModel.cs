using CommunityToolkit.Mvvm.Input;
using Desktop.Common;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public class TaskCreationViewModel : ViewModelBase
{
    public TaskCreationViewModel(
        IMessageNotifier messageNotifier,
        IEnumerable<ITaskRepository> taskRepositories)
    {
        SaveTask =
            new AsyncRelayCommand<(ICloseable, string, string)>(async args =>
            {
                var (closeable, taskName, taskDescription) = args;

                CreatedTask = new Task(taskName, taskDescription);

                foreach (var repository in taskRepositories)
                {
                    var creationResult = await repository.Save(CreatedTask);

                    if (!creationResult.Succeeded)
                    {
                        messageNotifier.Notify(
                            "Task creation has failed due to " +
                            "an internal error. The Task won't be created. " +
                            "Please, try again later.");

                        CreatedTask = null;

                        break;
                    }
                }

                closeable!.Close();
            });
    }

    public IRelayCommand SaveTask { get; }

    public Task? CreatedTask { get; private set; }
}