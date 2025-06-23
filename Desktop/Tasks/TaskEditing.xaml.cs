using System.Windows;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

using Task = Task;

public partial class TaskEditing : Window
{
    private readonly TaskEditingViewModel viewModel;

    public TaskEditing(Task original)
    {
        InitializeComponent();

        viewModel = new TaskEditingViewModel(original);

        Name.Text = original.Name;
        Description.Text = original.Description;
    }

    private void SaveTask(object sender, RoutedEventArgs e)
    {
        viewModel.Save.Execute((Name.Text, Description.Text));

        Close();
    }
}