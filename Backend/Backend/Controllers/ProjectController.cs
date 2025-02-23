using Microsoft.AspNetCore.Mvc;
using Task = Backend.Domain.Task;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetTasks")]
    public IEnumerable<Task> Get()
    {
        return
        [
            new Task("A task", "Dummy description"),
            new Task("Another task", "Dummy description"),
            new Task("Yet another task", "Dummy description"),
        ];
    }
}