using HotChocolateWorkshop.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotChocolateWorkshop.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : DbContext(options)
{
    public DbSet<Rocket> Rockets { get; init; }
}