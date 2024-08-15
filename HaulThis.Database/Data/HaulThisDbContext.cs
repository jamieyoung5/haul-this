using Microsoft.EntityFrameworkCore;
using HaulThis.Database.Models;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using HaulThis.Database.Models;
using Microsoft.Extensions.Logging;

namespace HaulThis.Database.Data;

public class HaulThisDbContext : DbContext
{
    public HaulThisDbContext() { }

    public HaulThisDbContext(DbContextOptions options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("HaulThis.Database.appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString("DevelopmentConnection"),            
            m => m.MigrationsAssembly("HaulThis.Migrations")
            ).LogTo(Console.WriteLine, LogLevel.Information);;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<Item>()
        //     .HasOne(i => i.PickupLocation)
        //     .WithMany()
        //     .HasForeignKey(i => i.PickupLocationID)
        //     .OnDelete(DeleteBehavior.Restrict);

        // modelBuilder.Entity<Item>()
        //     .HasOne(i => i.DeliveryLocation)
        //     .WithMany()
        //     .HasForeignKey(i => i.DeliveryLocationID)
        //     .OnDelete(DeleteBehavior.Restrict);
    }


    public DbSet<Confirmation> Confirmation { get; set; }
    public DbSet<Driver> Driver { get; set; }
    public DbSet<Item> Item { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<Trip> Trip { get; set; }
}
