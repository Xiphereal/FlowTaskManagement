using DomainTask = Backend.Domain.Task;

namespace Backend;

public interface ITaskRepository
{
    Task<IEnumerable<DomainTask>> All();
    Task Save(DomainTask toBeCreated);
}