using DomainTask = Backend.Domain.Task;

namespace Backend.Persistence;

public interface ITaskRepository
{
    Task<IEnumerable<DomainTask>> All();
    Task Save(DomainTask toBeCreated);
    Task Modify(DomainTask toBeModified);
    Task Delete(string taskName);
    Task<bool> Exist(string taskName);
    Task<bool> Exist(Guid task);
}