using Microsoft.EntityFrameworkCore;

namespace IndexCardBackendApi.Models;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<CardDeck> Decks { get; set; } = null!;

     public DbSet<Card> Cards { get; set; } = null!;
}