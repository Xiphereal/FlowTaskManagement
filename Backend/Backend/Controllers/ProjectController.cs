using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using DomainTask = Backend.Domain.Task;

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
        if (await taskRepository.Exist(toBeCreated.Id))
            return Results.Conflict();

        await taskRepository.Save(toBeCreated);

        return Results.Created();
    }

    [HttpPut("")]
    public async Task<IResult> Put(DomainTask toBeModified)
    {
        if (!await taskRepository.Exist(toBeModified.Id))
            return Results.NotFound(toBeModified);

        await taskRepository.Modify(toBeModified);

        return Results.Ok();
    }

    [HttpDelete("{task}")]
    public async Task<IResult> Delete(Guid task)
    {
        if (!await taskRepository.Exist(task))
            return Results.NotFound(task);

        await taskRepository.Delete(task);

        return Results.Ok();
    }
}