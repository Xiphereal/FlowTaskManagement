using DomainTask = Backend.Domain.Task;

namespace Backend.Persistence;

public interface ITaskRepository
{
    Task<IEnumerable<DomainTask>> All();
    Task Save(DomainTask toBeCreated);
    Task Delete(string taskName);
}