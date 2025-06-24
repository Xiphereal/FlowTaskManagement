namespace Desktop.Tasks;

public interface IRemoteTaskRepository
{
    Task<bool> IsAvailable();
}