using System.Windows;

namespace DRGiudflgh;

using Task = Domain.Task;

public partial class TaskCreation : Window
{
    public TaskCreation()
    {
        InitializeComponent();
    }

    public Task CreatedTask { get; set; }
}