using System.Windows;
using Desktop.Common;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

using Task = Task;

public partial class TaskCreation : Window, ICloseable
{
    public TaskCreation()
    {
        InitializeComponent();
    }

    public Task? CreatedTask { get; private set; }

    private void SaveTask(object sender, RoutedEventArgs e)
    {
        CreatedTask = new Task(name: Name.Text, description: Description.Text);
    }
}