using System.Windows;
using Desktop.Project;
using Desktop.Tasks;

namespace Desktop;

public partial class MainWindow : Window
{
    private readonly ITaskRepository taskRepository;

    public MainWindow(ITaskRepository taskRepository)
    {
        this.taskRepository = taskRepository;
        InitializeComponent();
        NavigateToTaskList();
    }

    private void NavigateToTaskList()
    {
        var tasksList = new TasksList(taskRepository);
        Content.Content = tasksList;
    }

    private void NavigateToTaskList(object sender, RoutedEventArgs e)
    {
        NavigateToTaskList();
    }

    private void NavigateToProjectTree(object sender, RoutedEventArgs e)
    {
        var projectTree = new ProjectTree();
        Content.Content = projectTree;
    }
}