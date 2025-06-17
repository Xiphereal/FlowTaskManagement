using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Desktop.Common;

namespace Desktop.Tasks;

public class TaskCreationViewModel : ViewModelBase
{
    public ICommand SaveTask { get; } =
        new RelayCommand<ICloseable>(closeable => { closeable.Close(); });
}