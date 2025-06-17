using System.Windows;
using Desktop.Common;

namespace Desktop.Tasks;

public partial class TaskCreation : Window, ICloseable, IShowable
{
    public TaskCreation(TaskCreationViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}