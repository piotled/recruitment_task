using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentTask.Api.DataAccess;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>().HasData(
            new Category()
            {
                Id = 1,
                Name = "Inny",
            },
            new Category()
            {
                Id = 2,
                Name = "Służbowy",
            },
            new Category()
            {
                Id = 3,
                Name = "Prywatny",
            }
        );

        builder.Entity<Subcategory>().HasData(
            new Subcategory()
            {
                Id = 1,
                Name = "Szef",
                CategoryId = 2,
            },
            new Subcategory()
            {
                Id = 2,
                Name = "Klient",
                CategoryId = 2,
            },
            new Subcategory()
            {
                Id = 3,
                Name = "Współpracownik",
                CategoryId = 2,
            },
            new Subcategory()
            {
                Id = 4,
                Name = "Rodzina",
                CategoryId = 3,
            },
            new Subcategory()
            {
                Id = 5,
                Name = "Znajomi",
                CategoryId = 3,
            }
        );
    }

    public DbSet<IssuedToken> IssuedTokens { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!; 
    public DbSet<Category> Categories { get; set; } = null!; 
    public DbSet<Subcategory> Subcategories { get; set; } = null!; 
}
