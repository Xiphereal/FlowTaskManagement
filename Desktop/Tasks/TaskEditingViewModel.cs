using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Desktop.Common;
using Task = Desktop.Domain.Task;

namespace Desktop.Tasks;

public class TaskEditingViewModel : ViewModelBase
{
    public TaskEditingViewModel(
        Task taskBeingEdited,
        IMessageNotifier messageNotifier,
        IEnumerable<ITaskRepository> taskRepositories)
    {
        Save = new AsyncRelayCommand<(string, string)>(async args =>
        {
            var (name, description) = args;

            var originalName = taskBeingEdited.Name;
            taskBeingEdited.Name = name;
            var originalDescription = taskBeingEdited.Description;
            taskBeingEdited.Description = description;

            foreach (var repository in taskRepositories)
            {
                var editingResult = await repository.Save(taskBeingEdited);

                if (!editingResult.Succeeded)
                {
                    messageNotifier.Notify(
                        "Task editing has failed due to " +
                        "an internal error. The modifications will be reverted. " +
                        "Please, try again later.");

                    taskBeingEdited.Name = originalName;
                    taskBeingEdited.Description = originalDescription;
                }
            }
        });
    }

    public ICommand Save { get; }
}