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
    private readonly TaskCreationViewModel taskCreationViewModel;

    public TasksList(
        ITaskRepository taskRepository,
        TaskListViewModel viewModel,
        TaskCreationViewModel taskCreationViewModel)
    {
        this.taskRepository = taskRepository;
        this.viewModel = viewModel;
        this.taskCreationViewModel = taskCreationViewModel;

        InitializeComponent();
        Tasks.ItemsSource = viewModel.Tasks;

        viewModel.PopulateTasks();
    }

    private void Add(object sender, RoutedEventArgs e)
    {
        new TaskCreation(taskCreationViewModel).ShowDialog();

        viewModel.Add.Execute(taskCreationViewModel);
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