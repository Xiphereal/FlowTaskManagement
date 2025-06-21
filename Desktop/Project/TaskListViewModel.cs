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
    private readonly ITaskRepository taskRepository;
    public ObservableCollection<Task> Tasks { get; } = [];

    public Func<TaskCreationViewModel, IShowable> TaskCreationViewCreator { get; set; }

    public TaskListViewModel(
        ITaskRepository taskRepository,
        TaskCreationViewModel taskCreationViewModel)
    {
        this.taskRepository = taskRepository;

        Add = new RelayCommand(() =>
        {
            TaskCreationViewCreator!(taskCreationViewModel).ShowDialog();

            if (taskCreationViewModel.CreatedTask is null)
                return;

            Tasks.Add(taskCreationViewModel.CreatedTask);
        });

        Edit = new RelayCommand<Task>(taskToEdit =>
        {
            var window = new TaskEditing(taskToEdit!);
            window.ShowDialog();
        });

        Delete = new RelayCommand<Task>(taskToRemove =>
        {
            Tasks.Remove(taskToRemove!);
            taskRepository.Delete(taskToRemove!);
        });
    }

    public ICommand Add { get; }

    public ICommand Edit { get; }
    public ICommand Delete { get; }

    public void PopulateTasks()
    {
        // TODO: load this async and deferred from the ctor.
        var retrievedTasks = SystemTask.Run(taskRepository.All).Result;
        foreach (var task in retrievedTasks)
            Tasks.Add(task);
    }
}