using System.Windows;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

using Task = Task;

public partial class TaskEditing : Window
{
    private readonly TaskEditingViewModel viewModel;

    public TaskEditing(Task original, IEnumerable<ITaskRepository> taskRepositories)
    {
        InitializeComponent();

        viewModel = new TaskEditingViewModel(original, taskRepositories);

        Name.Text = original.Name;
        Description.Text = original.Description;
    }

    private void SaveTask(object sender, RoutedEventArgs e)
    {
        viewModel.Save.Execute((Name.Text, Description.Text));

        Close();
    }
}