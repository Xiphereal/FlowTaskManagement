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
    public ObservableCollection<Task> Tasks { get; } = [];

    public Func<TaskCreationViewModel, IShowable> TaskCreationViewCreator { get; set; }

    public TaskListViewModel(
        IEnumerable<ITaskRepository> taskRepositories,
        TaskCreationViewModel taskCreationViewModel)
    {
        this.taskRepositories = taskRepositories;

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
                await repository.Delete(taskToRemove!);
        });
    }

    public ICommand Add { get; }

    public ICommand Delete { get; }

    public void PopulateTasks()
    {
        Tasks.Clear();

        // TODO: load this async and deferred from the ctor.
        var retrievedTasks = SystemTask.Run(() => SystemTask
                .WhenAll(taskRepositories.Select(x => x.All())))
            .Result
            .SelectMany(x => x.Tasks);
        foreach (var task in retrievedTasks.Distinct())
            Tasks.Add(task);
    }
}