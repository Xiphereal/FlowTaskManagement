using System.Windows;

namespace DRGiudflgh;

using Task = Domain.Task;

public partial class TaskEditing : Window
{
    public TaskEditing(Task original)
    {
        InitializeComponent();
        
        this.original = original;
        Name.Text = original.Name;
        Description.Text = original.Description;
    }

    private readonly Task original;

    public Task? EditedTask { get; private set; }

    private void SaveTask(object sender, RoutedEventArgs e)
    {
        original.Name = Name.Text;
        original.Description = Description.Text;
        
        Close();
    }
}