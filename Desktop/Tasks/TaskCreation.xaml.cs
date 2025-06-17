using System.Windows;
using Desktop.Common;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

using Task = Task;

public partial class TaskCreation : Window, ICloseable
{
    private readonly TaskCreationViewModel viewModel;

    public TaskCreation(TaskCreationViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        this.viewModel = viewModel;
    }

    public Task? CreatedTask => viewModel.CreatedTask;
}