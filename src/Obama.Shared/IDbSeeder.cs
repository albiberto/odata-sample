using Microsoft.EntityFrameworkCore;

namespace Obama.Infrastructure.DevSpace;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}