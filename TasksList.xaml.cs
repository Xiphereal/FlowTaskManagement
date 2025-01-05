using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Desktop;

using Task = Domain.Task;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class TasksList : Window
{
    private readonly ObservableCollection<Task> tasks = [];

    public TasksList()
    {
        InitializeComponent();
        Tasks.ItemsSource = tasks;

        tasks.Add(new Task("DummyName", "DummyDescription"));
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

        if (window.EditedTask is not null) 
            tasks.Add(window.EditedTask);
    }

    private void Delete(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;
        
        var toRemove = button.DataContext as Task;
        
        tasks.Remove(toRemove!);
    }
}