using System.Threading.Tasks;
using Desktop.Tasks;

namespace Desktop.Tests.Integration;

/// <summary>
///     This serves the purpose of defining the contract of which behaviour should
///     any <see cref="ITaskRepository" /> exhibit.
/// </summary>
public interface ITaskRepositoryTests
{
    Task NoTaskArePersisted_ReturnsNothing();
    Task CanLoadPersistedTasks();
    Task CanLoadSeveralPersistedTasks();
    Task CanPersistTasksWhenNoOtherExistedBefore();
    Task PersistNewTaskDoesNotOverrideExistingOnes();
    Task TasksCanBeModified();
    Task CanDeleteExistingTasks();
    Task DeletesJustTheRequestedTask_KeepingTheOtherOnes();
}