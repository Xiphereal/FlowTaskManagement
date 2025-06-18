using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Tasks;
using Task = Desktop.Domain.Task;

namespace Desktop.Project;

public partial class TasksList : UserControl
{
    public TasksList(TaskListViewModel viewModel)
    {
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
}