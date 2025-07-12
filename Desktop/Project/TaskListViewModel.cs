using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Desktop.Common;
using Desktop.Tasks;
using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Desktop.Project;

public class TaskListViewModel : ViewModelBase
{
    private readonly IEnumerable<ITaskRepository> taskRepositories;
    private readonly IMessageNotifier messageNotifier;
    public ObservableCollection<Task> Tasks { get; } = [];

    public Func<TaskCreationViewModel, IShowable> TaskCreationViewCreator { get; set; }

    public TaskListViewModel(
        IEnumerable<ITaskRepository> taskRepositories,
        TaskCreationViewModel taskCreationViewModel,
        IMessageNotifier messageNotifier)
    {
        this.taskRepositories = taskRepositories;
        this.messageNotifier = messageNotifier;

        Add = new RelayCommand(() =>
        {
            TaskCreationViewCreator!(taskCreationViewModel).ShowDialog();

            if (taskCreationViewModel.CreatedTask is null)
                return;

            Tasks.Add(taskCreationViewModel.CreatedTask);
        });

        Delete = new AsyncRelayCommand<Task>(async taskToRemove =>
        {
            Tasks.Remove(taskToRemove!);

            foreach (var repository in this.taskRepositories)
            {
                var result = await repository.Delete(taskToRemove!);

                if (!result.Succeeded)
                    messageNotifier.Notify(
                        "Task deletion has failed due to " +
                        "an internal error. The Task won't be deleted. " +
                        "Please, try again later.");
            }
        });
    }

    public ICommand Add { get; }

    public ICommand Delete { get; }

    public void PopulateTasks()
    {
        Tasks.Clear();

        // TODO: load this async and deferred from the ctor.
        var queryResult = SystemTask.Run(() => SystemTask
                .WhenAll(taskRepositories.Select(x => x.All())))
            .Result;

        if (queryResult.Any(x => !x.Succeeded))
            messageNotifier.Notify(
                "Task retrieval has failed due to " +
                "an internal error. Expect some Tasks to be missing. " +
                "Please, try again later.");

        var retrievedTasks = queryResult
            .SelectMany(x => x.Tasks);
        foreach (var task in retrievedTasks.Distinct())
            Tasks.Add(task);
    }
}