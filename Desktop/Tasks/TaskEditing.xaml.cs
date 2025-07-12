using System.Windows;
using Desktop.Common;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

using Task = Task;

public partial class TaskEditing : Window
{
    private readonly TaskEditingViewModel viewModel;

    public TaskEditing(
        Task original,
        IMessageNotifier messageNotifier,
        IEnumerable<ITaskRepository> taskRepositories)
    {
        InitializeComponent();

        viewModel = new TaskEditingViewModel(original, messageNotifier, taskRepositories);

        Name.Text = original.Name;
        Description.Text = original.Description;
    }

    private void SaveTask(object sender, RoutedEventArgs e)
    {
        viewModel.Save.Execute((Name.Text, Description.Text));

        Close();
    }
}