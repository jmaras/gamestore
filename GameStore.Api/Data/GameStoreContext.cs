using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new {Id = 1, Name = "Fighting"},
            new {Id = 2, Name = "RPG"},
            new {Id = 3, Name = "Action-RPG"},
            new {Id = 4, Name = "Action-Adventure"},
            new {Id = 5, Name = "Sports"},
            new {Id = 6, Name = "Racing"},
            new {Id = 7, Name = "Kids and Family"}
        );
    }
}
