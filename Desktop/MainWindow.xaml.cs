using System.Windows;
using Desktop.Project;
using Desktop.Tasks;

namespace Desktop;

public partial class MainWindow : Window
{
    private readonly ITaskRepository taskRepository;
    private readonly TaskCreationViewModel taskCreationViewModel;
    private readonly TaskListViewModel taskListViewModel;

    public MainWindow(
        ITaskRepository taskRepository,
        TaskCreationViewModel taskCreationViewModel,
        TaskListViewModel taskListViewModel)
    {
        this.taskRepository = taskRepository;
        this.taskCreationViewModel = taskCreationViewModel;
        this.taskListViewModel = taskListViewModel;
        InitializeComponent();
        NavigateToTaskList();
    }

    private void NavigateToTaskList()
    {
        var tasksList = new TasksList(
            taskRepository,
            taskListViewModel);
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