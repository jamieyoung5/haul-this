using Microsoft.EntityFrameworkCore;

namespace HaulThis;

public class HaulThisDbContext : DbContext
{
    public HaulThisDbContext() { }

   public HaulThisDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Confirmation> Confirmation { get; set; }
    public DbSet<Driver> Driver { get; set; }
    public DbSet<Item> Item { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<Trip> Trip { get; set; }
}
