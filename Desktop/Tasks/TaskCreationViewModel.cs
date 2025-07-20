using CommunityToolkit.Mvvm.Input;
using Desktop.Common;
using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

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

                var savingTasks = taskRepositories
                    .Select(x => x.Save(CreatedTask));
                var results = await SystemTask.WhenAll(savingTasks);

                if (results.Any(x => !x.Succeeded))
                    messageNotifier.Notify(
                        "Task creation has failed due to " +
                        "an internal error. The Task won't be created. " +
                        "Please, try again later.");

                if (results.All(x => !x.Succeeded))
                    CreatedTask = null;

                closeable!.Close();
            });
    }

    public IRelayCommand SaveTask { get; }

    public Task? CreatedTask { get; private set; }
}