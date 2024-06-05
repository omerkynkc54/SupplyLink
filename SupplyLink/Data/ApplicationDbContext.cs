using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SupplyLink.Models;

namespace SupplyLink.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<OrderType> OrderTypes { get; set; }
    public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data for enumerations
        modelBuilder.Entity<UnitOfMeasure>().HasData(
            new UnitOfMeasure { Id = 1, Name = "Milimetre" },
            new UnitOfMeasure { Id = 2, Name = "Metre" },
            new UnitOfMeasure { Id = 3, Name = "Boy" },
            new UnitOfMeasure { Id = 4, Name = "Litre" },
            new UnitOfMeasure { Id = 5, Name = "Galon" },
            new UnitOfMeasure { Id = 6, Name = "Adet" },
            new UnitOfMeasure { Id = 7, Name = "Teneke" }
        );

        modelBuilder.Entity<OrderType>().HasData(
            new UnitOfMeasure { Id = 1, Name = "Sarf" },
            new UnitOfMeasure { Id = 2, Name = "Montaj" },
            new UnitOfMeasure { Id = 3, Name = "İmalat Dışı" },
            new UnitOfMeasure { Id = 4, Name = "Kaynakhane" }
        );

        modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus { Id = 1, Name = "Onaylandı" },
            new OrderStatus { Id = 2, Name = "Ürün Bekleniyor" },
            new OrderStatus { Id = 3, Name = "Teslim Edildi" },
            new OrderStatus { Id = 4, Name = "İptal Edildi" }
        );
    }

}