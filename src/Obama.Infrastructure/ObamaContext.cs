using Microsoft.EntityFrameworkCore;
using Obama.Domain;

namespace Obama.Infrastructure;

public class ObamaContext(DbContextOptions<ObamaContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; init; }
    public DbSet<Role> Roles { get; init; }
}