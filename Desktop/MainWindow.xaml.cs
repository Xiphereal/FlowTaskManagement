using System.Windows;
using Desktop.Project;
using Desktop.Tasks;

namespace Desktop;

public partial class MainWindow : Window
{
    private readonly ITaskRepository taskRepository;
    private readonly TaskCreationViewModel taskCreationViewModel;

    public MainWindow(
        ITaskRepository taskRepository,
        TaskCreationViewModel taskCreationViewModel)
    {
        this.taskRepository = taskRepository;
        this.taskCreationViewModel = taskCreationViewModel;
        InitializeComponent();
        NavigateToTaskList();
    }

    private void NavigateToTaskList()
    {
        var tasksList = new TasksList(taskRepository, taskCreationViewModel);
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