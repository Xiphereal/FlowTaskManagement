using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Desktop.Common;
using Task = Desktop.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

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

            var editsTasks = taskRepositories
                .Select(x => x.Save(taskBeingEdited));
            var results = await SystemTask.WhenAll(editsTasks);

            if (results.Any(x => !x.Succeeded))
                messageNotifier.Notify(
                    "Task editing has failed due to " +
                    "an internal error. The modifications will be reverted. " +
                    "Please, try again later.");

            if (results.All(x => !x.Succeeded))
                RevertEdit(taskBeingEdited, originalName, originalDescription);
        });
    }

    private static void RevertEdit(
        Task taskBeingEdited,
        string originalName,
        string originalDescription)
    {
        taskBeingEdited.Name = originalName;
        taskBeingEdited.Description = originalDescription;
    }

    public ICommand Save { get; }
}