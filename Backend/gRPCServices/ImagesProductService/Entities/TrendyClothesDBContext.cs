using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ImagesProductService.Entities;

public partial class TrendyClothesDBContext : DbContext
{
    public TrendyClothesDBContext()
    {
    }

    public TrendyClothesDBContext(DbContextOptions<TrendyClothesDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AuctionsProduct> AuctionsProducts { get; set; }

    public virtual DbSet<CategoriesProduct> CategoriesProducts { get; set; }

    public virtual DbSet<PhotosProduct> PhotosProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<QAProduct> QAProducts { get; set; }

    public virtual DbSet<RolesUser> RolesUsers { get; set; }

    public virtual DbSet<StatusesAuction> StatusesAuctions { get; set; }

    public virtual DbSet<StatusesProduct> StatusesProducts { get; set; }

    public virtual DbSet<TypesProduct> TypesProducts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<User_Address> User_Addresses { get; set; }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            IConfigurationRoot? config = new ConfigurationBuilder ()
                .AddJsonFile ("appsettings.json")
                .Build ();

            string? connectionString = config.GetConnectionString ("DefaultConnection");

            optionsBuilder.UseSqlServer (connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC0740C62C8B");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.ExtNumber)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.IntNumber)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Neighborhood)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AuctionsProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auctions__3214EC0723A121B2");

            entity.ToTable("AuctionsProduct");

            entity.Property(e => e.FirstPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.LastPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.MinBid).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Buyer).WithMany(p => p.AuctionsProducts)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK_BuyerProduct_Product");

            entity.HasOne(d => d.Product).WithMany(p => p.AuctionsProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuctionProduct_Product");

            entity.HasOne(d => d.Status).WithMany(p => p.AuctionsProducts)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusAuction_Auction");
        });

        modelBuilder.Entity<CategoriesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07A0E42DC8");

            entity.ToTable("CategoriesProduct");

            entity.HasIndex(e => e.Category, "UQ__Categori__4BB73C324C743DD9").IsUnique();

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhotosProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotosPr__3214EC0776A97EAF");

            entity.ToTable("PhotosProduct");

            entity.Property(e => e.Mime)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.PhotosProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhotoProduct_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07182DDB1B");

            entity.Property(e => e.AverageStars).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Discount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryProduct_Product");

            entity.HasOne(d => d.Seller).WithMany(p => p.Products)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seller_Product");

            entity.HasOne(d => d.Status).WithMany(p => p.Products)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusProduct_Product");

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TypeProduct_Product");
        });

        modelBuilder.Entity<QAProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QAProduc__3214EC07972010A8");

            entity.ToTable("QAProduct");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Stars).HasColumnType("decimal(2, 1)");

            entity.HasOne(d => d.Product).WithMany(p => p.QAProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QAProduct_Product");

            entity.HasOne(d => d.User).WithMany(p => p.QAProducts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserQA_Product");
        });

        modelBuilder.Entity<RolesUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolesUse__3214EC07980CE4C4");

            entity.ToTable("RolesUser");

            entity.HasIndex(e => e.Role, "UQ__RolesUse__DA15413EE052EFD8").IsUnique();

            entity.Property(e => e.Role)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusesAuction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statuses__3214EC07DE1C5768");

            entity.ToTable("StatusesAuction");

            entity.HasIndex(e => e.Status, "UQ__Statuses__3A15923F04B7B2F9").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statuses__3214EC07E8DBE716");

            entity.ToTable("StatusesProduct");

            entity.HasIndex(e => e.Status, "UQ__Statuses__3A15923FCE66BA06").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TypesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypesPro__3214EC073D0D7E8F");

            entity.ToTable("TypesProduct");

            entity.HasIndex(e => e.Type, "UQ__TypesPro__F9B8A48BA498FDFE").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07D853FCB5");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E43C5B369B").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534896B42BE").IsUnique();

            entity.Property(e => e.AreaCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TwoFactorCode).HasMaxLength(6);
            entity.Property(e => e.Username)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleUser_User");
        });

        modelBuilder.Entity<User_Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User_Add__3214EC07F92AC831");

            entity.ToTable("User_Address");

            entity.HasOne(d => d.Address).WithMany(p => p.User_Addresses)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_UserAddress_Address");

            entity.HasOne(d => d.User).WithMany(p => p.User_Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserAddress_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
