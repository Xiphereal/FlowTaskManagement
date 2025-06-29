using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Desktop.Common;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public class TaskEditingViewModel : ViewModelBase
{
    public TaskEditingViewModel(
        Task taskBeingEdited,
        IEnumerable<ITaskRepository> taskRepositories)
    {
        Save = new RelayCommand<(string, string)>(args =>
        {
            var (name, description) = args;

            taskBeingEdited.Name = name;
            taskBeingEdited.Description = description;

            foreach (var repository in taskRepositories)
                repository.Save(taskBeingEdited);
        });
    }

    public ICommand Save { get; }
}