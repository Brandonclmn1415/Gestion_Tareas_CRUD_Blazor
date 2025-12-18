using Microsoft.EntityFrameworkCore;

namespace SIG_T.Persistence.Data;

/// <summary>
/// Test helper to create <see cref="ApplicationDbContext"/> instances from a connection string.
/// </summary>
public static class TestDbContextFactory
{
    public static ApplicationDbContext Create(string connectionString)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}