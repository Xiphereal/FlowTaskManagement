using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Task = Backend.Domain.Task;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpGet("GetTasks")]
    public async Task<IEnumerable<Task>> Get()
    {
        return await taskRepository.All();
    }

    [HttpPost("SaveTask")]
    public async Task<IResult> Post(Task toBeCreated)
    {
        await taskRepository.Save(toBeCreated);

        return Results.Created();
    }
}