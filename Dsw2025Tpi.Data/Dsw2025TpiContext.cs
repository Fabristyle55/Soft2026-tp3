using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext: DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your entities here
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products"); // Specify the table name if needed

            entity.HasKey(p => p.Id);
            entity.Property(p => p.sku).IsRequired().HasMaxLength(50);
            entity.Property(p => p.internalCode).IsRequired().HasMaxLength(50);
            entity.Property(p => p.name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.description).HasMaxLength(500);
            entity.Property(p => p.currentUnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(p => p.stockQuantity).IsRequired();
            entity.Property(p => p.isActive).IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders"); // Specify the table name if needed

            entity.HasKey(o => o.Id);
            entity.Property(o => o.date).IsRequired();
            entity.Property(o => o.shippingAdress).IsRequired().HasMaxLength(200);
            entity.Property(o => o.billingAdress).IsRequired().HasMaxLength(200);
            entity.Property(o => o.notes).HasMaxLength(500);

        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems"); // Specify the table name if needed

            entity.HasKey(oi => oi.Id);
            entity.Property(oi => oi.quantity).IsRequired();
            entity.Property(oi => oi.unitPrice).HasColumnType("decimal(18,2)");

        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnType("char(36)");
            entity.Property(c => c.name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.email).IsRequired().HasMaxLength(100);
            entity.Property(c => c.phoneNumber).IsRequired().HasMaxLength(20);
        });

        //clase dbContext 1:34 hr
    }


}
