using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence;

/// <summary>
///     This eases the local testing by automatically ensuring that the database is
///     up to date. Otherwise, it will need to be done by hand each time the local
///     env is cleared.
/// </summary>
/// <remarks>
///     This is only for testing environments. In production-like scenarios,
///     running database migrations or creating/deleting it should be delegated to
///     the CI/CD.
/// </remarks>
public class TestingDatabaseInitializer
{
    private readonly BackendContext context;

    public TestingDatabaseInitializer(BackendContext context)
    {
        this.context = context;
    }

    public void Execute()
    {
        context.Database.Migrate();
    }
}