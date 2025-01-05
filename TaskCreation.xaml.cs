using System.Windows;

namespace DRGiudflgh;

using Task = Domain.Task;

public partial class TaskCreation : Window
{
    public TaskCreation()
    {
        InitializeComponent();
    }

    public Task? CreatedTask { get; private set; }

    private void SaveTask(object sender, RoutedEventArgs e)
    {
        CreatedTask = new Task(Name: Name.Text, Description: Description.Text);
        Close();
    }
}