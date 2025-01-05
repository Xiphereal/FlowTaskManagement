using System.Collections.ObjectModel;
using System.Windows;

namespace DRGiudflgh;

using Task = Domain.Task;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ObservableCollection<Task> tasks = [];

    public MainWindow()
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
}