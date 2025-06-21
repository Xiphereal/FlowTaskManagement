using System.Windows.Controls;
using Desktop.Tasks;

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
}