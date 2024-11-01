using System;
using System.Collections.Generic;
using DiamondShop.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DiamondShop.Repository;

public partial class Prn231DiamondShopContext : DbContext
{
    public Prn231DiamondShopContext()
    {
    }

    public Prn231DiamondShopContext(DbContextOptions<Prn231DiamondShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Diamond> Diamonds { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07A42D7653");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.AvatarUrl).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValue("available");
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07FAC0ABFC");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LastUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("available");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC0747D39E1E");

            entity.ToTable("Certificate");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CaratWeight).HasMaxLength(50);
            entity.Property(e => e.Clarity).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Cut).HasMaxLength(50);
            entity.Property(e => e.DateOfIssue)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Origin).HasMaxLength(50);
            entity.Property(e => e.Shape).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("available");
        });

        modelBuilder.Entity<Diamond>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diamond__3214EC07E8F6F9B2");

            entity.ToTable("Diamond");

            entity.HasIndex(e => e.CertificateId, "UQ__Diamond__BBF8A7C0B454C0A5").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CaratWeight).HasMaxLength(50);
            entity.Property(e => e.Clarity).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Cut).HasMaxLength(50);
            entity.Property(e => e.LastUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Origin).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Shape).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("available");

            entity.HasOne(d => d.Certificate).WithOne(p => p.Diamond)
                .HasForeignKey<Diamond>(d => d.CertificateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diamond__Certifi__38996AB5");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC0754BE159D");

            entity.ToTable("Order");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PayMethod).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__4CA06362");

            entity.HasOne(d => d.DeliveryStaff).WithMany(p => p.OrderDeliveryStaffs)
                .HasForeignKey(d => d.DeliveryStaffId)
                .HasConstraintName("FK__Order__DeliveryS__4E88ABD4");

            entity.HasOne(d => d.SalesStaff).WithMany(p => p.OrderSalesStaffs)
                .HasForeignKey(d => d.SalesStaffId)
                .HasConstraintName("FK__Order__SalesStaf__4D94879B");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC074A34E995");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__52593CB8");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__Produ__534D60F1");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Picture__3214EC075CF96B5F");

            entity.ToTable("Picture");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Diamond).WithMany(p => p.Pictures)
                .HasForeignKey(d => d.DiamondId)
                .HasConstraintName("FK__Picture__Diamond__46E78A0C");

            entity.HasOne(d => d.Product).WithMany(p => p.Pictures)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Picture__Product__47DBAE45");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC078C456B8B");

            entity.ToTable("Product");

            entity.HasIndex(e => e.DiamondId, "UQ__Product__23A8E79A65BC81A2").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LastUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Material).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("available");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__4316F928");

            entity.HasOne(d => d.Diamond).WithOne(p => p.Product)
                .HasForeignKey<Product>(d => d.DiamondId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Diamond__4222D4EF");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC0728121297");

            entity.ToTable("Promotion");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ExpiredDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("available");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
