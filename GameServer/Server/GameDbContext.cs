using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using SharedLibrary;
using SharedLibrary.models;

namespace Server;

public class GameDbContext : IdentityDbContext<User>
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Plot>().Property(p => p.Tile_Data)
            .HasColumnType("MediumBlob");

        modelBuilder.Entity<User>().HasData(
            new User()
            {
                AccessFailedCount = 2, Role = ""
                ,Salt = "" , 
            }
        );
    }
    
    public DbSet<Mariana> Marianas { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<UserMap> UserMaps { get; set; }
    public DbSet<Plot> Plots { get; set; }
    public DbSet<Structure> Structures { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Reward> Rewards { get; set; }
}