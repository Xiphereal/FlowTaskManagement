using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using DomainTask = Backend.Domain.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]/tasks")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;
    private readonly ITaskRepository taskRepository;

    public ProjectController(
        ILogger<ProjectController> logger,
        ITaskRepository taskRepository)
    {
        this.taskRepository = taskRepository;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IEnumerable<DomainTask>> Get()
    {
        return await taskRepository.All();
    }

    [HttpPost("")]
    public async Task<IResult> Post(DomainTask toBeCreated)
    {
        await taskRepository.Save(toBeCreated);

        return Results.Created();
    }

    [HttpDelete("{taskName}")]
    public async SystemTask Delete(string taskName)
    {
        await taskRepository.Delete(taskName);
    }
}