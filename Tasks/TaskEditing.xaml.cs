using System.Windows;

namespace Desktop.Tasks;

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

    private void SaveTask(object sender, RoutedEventArgs e)
    {
        original.Name = Name.Text;
        original.Description = Description.Text;
        
        Close();
    }
}