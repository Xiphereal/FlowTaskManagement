using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Tasks;

namespace Desktop.Project;

using Task = Domain.Task;

public partial class TasksList : UserControl
{
    private readonly ObservableCollection<Task> tasks = [];

    public TasksList(ITaskRepository taskRepository)
    {
        InitializeComponent();
        Tasks.ItemsSource = tasks;

        var retrievedTasks = taskRepository.All();
        foreach (var task in retrievedTasks)
            tasks.Add(task);
    }

    private void Add(object sender, RoutedEventArgs e)
    {
        var window = new TaskCreation();
        window.ShowDialog();

        if (window.CreatedTask is not null) 
            tasks.Add(window.CreatedTask);
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
    }
}