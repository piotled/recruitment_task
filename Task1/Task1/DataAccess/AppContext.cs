using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Task1.DataAccess;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().HasMany(e => e.Contacts)
            .WithOne(e => e.Owner)
            .HasForeignKey(e => e.OwnerId);
        builder.Entity<Contact>()
            .HasOne(e => e.ReferencedUser)
            .WithMany();

    }

    public DbSet<IssuedToken> IssuedTokens { get; set; } = null!;
    public new DbSet<User> Users { get; set; } = null!; 
}
