using System.Windows;
using Desktop.Project;
using Desktop.Tasks;

namespace Desktop;

public partial class MainWindow : Window
{
    private readonly TaskListViewModel taskListViewModel;
    private readonly IEnumerable<ITaskRepository> taskRepositories;

    public MainWindow(
        TaskListViewModel taskListViewModel,
        MainWindowViewModel viewModel,
        IEnumerable<ITaskRepository> taskRepositories)
    {
        this.taskListViewModel = taskListViewModel;
        this.taskRepositories = taskRepositories;
        DataContext = viewModel;

        InitializeComponent();
        NavigateToTaskList();
    }

    private void NavigateToTaskList()
    {
        var tasksList = new TasksList(taskListViewModel, taskRepositories);
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