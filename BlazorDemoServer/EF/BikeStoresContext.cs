using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorDemo.Server;

public partial class BikeStoresContext : DbContext
{
    public BikeStoresContext()
    {
    }

    public BikeStoresContext(DbContextOptions<BikeStoresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Staff> Staffs { get; set; }
    public virtual DbSet<Stock> Stocks { get; set; }
    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=tcp:sysdev-sqlserver-public.database.windows.net,1433;Initial Catalog=BikeStores;Persist Security Info=False;User ID=fvv;Password=azimuth6@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("brands", "production");

            entity.Property(e => e.BrandId).HasColumnName("brand_id");

            entity.Property(e => e.BrandName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("brand_name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories", "production");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.Property(e => e.CategoryName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers", "sales");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");

            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.Property(e => e.State)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("state");

            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("street");

            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("zip_code");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders", "sales");

            entity.Property(e => e.OrderId).HasColumnName("order_id");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");

            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("order_date");

            entity.Property(e => e.OrderStatus).HasColumnName("order_status");

            entity.Property(e => e.RequiredDate)
                .HasColumnType("date")
                .HasColumnName("required_date");

            entity.Property(e => e.ShippedDate)
                .HasColumnType("date")
                .HasColumnName("shipped_date");

            entity.Property(e => e.StaffId).HasColumnName("staff_id");

            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__orders__customer__6D0D32F4");

            entity.HasOne(d => d.Staff)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__staff_id__6EF57B66");

            entity.HasOne(d => d.Store)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__orders__store_id__6E01572D");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ItemId })
                .HasName("PK__order_it__837942D4137ACFD9");

            entity.ToTable("order_items", "sales");

            entity.Property(e => e.OrderId).HasColumnName("order_id");

            entity.Property(e => e.ItemId).HasColumnName("item_id");

            entity.Property(e => e.Discount)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("discount");

            entity.Property(e => e.ListPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("list_price");

            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__order_ite__order__72C60C4A");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__order_ite__produ__73BA3083");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products", "production");

            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.Property(e => e.BrandId).HasColumnName("brand_id");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.Property(e => e.ListPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("list_price");

            entity.Property(e => e.ModelYear).HasColumnName("model_year");

            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("product_name");

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__products__brand___619B8048");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__products__catego__60A75C0F");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("staffs", "sales");

            entity.HasIndex(e => e.Email, "UQ__staffs__AB6E6164175C985B")
                .IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("staff_id");

            entity.Property(e => e.Active).HasColumnName("active");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");

            entity.Property(e => e.ManagerId).HasColumnName("manager_id");

            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.HasOne(d => d.Manager)
                .WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__staffs__manager___6A30C649");

            entity.HasOne(d => d.Store)
                .WithMany(p => p.staff)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__staffs__store_id__693CA210");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.ProductId })
                .HasName("PK__stocks__E68284D32C54BDB7");

            entity.ToTable("stocks", "production");

            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__stocks__product___778AC167");

            entity.HasOne(d => d.Store)
                .WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__stocks__store_id__76969D2E");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("stores", "sales");

            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("city");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");

            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.Property(e => e.State)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("state");

            entity.Property(e => e.StoreName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("store_name");

            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("street");

            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("zip_code");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
