using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Tasks;
using Task = Desktop.Domain.Task;

namespace Desktop.Project;

public partial class TasksList : UserControl
{
    private readonly ITaskRepository taskRepository;
    private readonly TaskListViewModel viewModel;

    public TasksList(
        ITaskRepository taskRepository,
        TaskListViewModel viewModel)
    {
        this.taskRepository = taskRepository;
        this.viewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
        Tasks.ItemsSource = viewModel.Tasks;

        viewModel.PopulateTasks();
        viewModel.TaskCreationViewCreator = creationViewModel =>
            new TaskCreation(creationViewModel);
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

        viewModel.Tasks.Remove(toRemove!);
        taskRepository.Delete(toRemove!);
    }
}