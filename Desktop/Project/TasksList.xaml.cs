using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Tasks;
using Task = Desktop.Domain.Task;

namespace Desktop.Project;

using SystemTask = System.Threading.Tasks.Task;

public partial class TasksList : UserControl
{
    private readonly ITaskRepository taskRepository;
    private readonly TaskCreationViewModel taskCreationViewModel;
    private readonly ObservableCollection<Task> tasks = [];

    public TasksList(
        ITaskRepository taskRepository,
        TaskCreationViewModel taskCreationViewModel)
    {
        this.taskRepository = taskRepository;
        this.taskCreationViewModel = taskCreationViewModel;

        InitializeComponent();
        Tasks.ItemsSource = tasks;

        // TODO: load this async and deferred from the ctor.
        var retrievedTasks = SystemTask.Run(taskRepository.All).Result;
        foreach (var task in retrievedTasks)
            tasks.Add(task);
    }

    private void Add(object sender, RoutedEventArgs e)
    {
        var taskCreationWindow = new TaskCreation(taskCreationViewModel);
        taskCreationWindow.ShowDialog();

        if (taskCreationWindow.CreatedTask is null)
            return;

        tasks.Add(taskCreationWindow.CreatedTask);
    }

    private void Edit(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListBoxItem { DataContext: Task task })
            return;

        var window = new TaskEditing(task);
        window.ShowDialog();
    }

    private void Delete(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        var toRemove = button.DataContext as Task;

        tasks.Remove(toRemove!);
        taskRepository.Delete(toRemove!);
    }
}