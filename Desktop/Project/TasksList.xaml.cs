using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Tasks;
using Task = Desktop.Domain.Task;

namespace Desktop.Project;

public partial class TasksList : UserControl
{
    public TasksList(TaskListViewModel viewModel)
    {
        DataContext = viewModel;

        InitializeComponent();
        Tasks.ItemsSource = viewModel.Tasks;

        viewModel.PopulateTasks();
        viewModel.TaskCreationViewCreator = creationViewModel =>
            new TaskCreation(creationViewModel);
    }

    /// <remarks>
    ///     This event handler in the code behind complies with the MVVM architectural
    ///     style: the View (which this code behind is part of) only has concerns of
    ///     the View.
    ///     <para>
    ///         If this event handler had code related with the control of the use
    ///         case, it will be moved to a ViewModel (since it is its responsibility).
    ///     </para>
    /// </remarks>
    private void Edit(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListBoxItem { DataContext: Task task })
            return;

        var window = new TaskEditing(task);
        window.ShowDialog();
    }
}