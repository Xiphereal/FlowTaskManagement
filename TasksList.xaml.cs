using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DRGiudflgh;

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

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new TaskCreation();
        window.ShowDialog();

        if (window.CreatedTask is not null) 
            tasks.Add(window.CreatedTask);
    }

    private void OpenTaskForEditing(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListBoxItem { DataContext: Task task })
            return;
        
        var window = new TaskEditing(task);
        window.ShowDialog();

        if (window.EditedTask is not null) 
            tasks.Add(window.EditedTask);
    }
}