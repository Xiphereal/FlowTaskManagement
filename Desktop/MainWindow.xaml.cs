using System.Windows;
using Desktop.Project;

namespace Desktop;

public partial class MainWindow : Window
{
    private readonly TaskListViewModel taskListViewModel;

    public MainWindow(TaskListViewModel taskListViewModel)
    {
        this.taskListViewModel = taskListViewModel;
        InitializeComponent();
        NavigateToTaskList();
    }

    private void NavigateToTaskList()
    {
        var tasksList = new TasksList(taskListViewModel);
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