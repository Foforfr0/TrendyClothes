using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuctionParticipantService.Entities;

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

    public virtual DbSet<BidsAuction> BidsAuctions { get; set; }

    public virtual DbSet<CategoriesProduct> CategoriesProducts { get; set; }

    public virtual DbSet<PhotosAuction> PhotosAuctions { get; set; }

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
            var environment = Environment.GetEnvironmentVariable ("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var config = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json", optional: false)
                .AddJsonFile ($"appsettings.{environment}.json", optional: true) // <- Este es el cambio
                .Build ();

            var connectionString = config.GetConnectionString ("SQLServer");

            optionsBuilder.UseSqlServer (connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC07A7FC68B9");

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
            entity.HasKey(e => e.Id).HasName("PK__Auctions__3214EC070E909E9D");

            entity.ToTable("AuctionsProduct");

            entity.Property(e => e.Bid).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.FirstPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.LastPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Seller).WithMany(p => p.AuctionsProducts)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK_BuyerProduct_Product");

            entity.HasOne(d => d.Status).WithMany(p => p.AuctionsProducts)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_StatusAuction_Auction");
        });

        modelBuilder.Entity<BidsAuction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BidsAuct__3214EC07459870EC");

            entity.ToTable("BidsAuction");

            entity.HasOne(d => d.Auction).WithMany(p => p.BidsAuctions)
                .HasForeignKey(d => d.AuctionId)
                .HasConstraintName("FK_BidAuction_Auction");

            entity.HasOne(d => d.Buyer).WithMany(p => p.BidsAuctions)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BidUser_Auction");
        });

        modelBuilder.Entity<CategoriesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07D96AC211");

            entity.ToTable("CategoriesProduct");

            entity.HasIndex(e => e.Category, "UQ__Categori__4BB73C329F210616").IsUnique();

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhotosAuction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotosAu__3214EC0700BEFC19");

            entity.ToTable("PhotosAuction");

            entity.Property(e => e.Mime)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Auction).WithMany(p => p.PhotosAuctions)
                .HasForeignKey(d => d.AuctionId)
                .HasConstraintName("FK_PhotoAuction_Auction");
        });

        modelBuilder.Entity<PhotosProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotosPr__3214EC0723455FFE");

            entity.ToTable("PhotosProduct");

            entity.Property(e => e.Mime)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.PhotosProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_PhotoProduct_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC0740E10207");

            entity.Property(e => e.AverageStars).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.Description).IsUnicode(false);
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
            entity.HasKey(e => e.Id).HasName("PK__QAProduc__3214EC07E55D11E4");

            entity.ToTable("QAProduct");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Stars).HasColumnType("decimal(2, 1)");

            entity.HasOne(d => d.Product).WithMany(p => p.QAProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_QAProduct_Product");

            entity.HasOne(d => d.User).WithMany(p => p.QAProducts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserQA_Product");
        });

        modelBuilder.Entity<RolesUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolesUse__3214EC07428D761F");

            entity.ToTable("RolesUser");

            entity.HasIndex(e => e.Role, "UQ__RolesUse__DA15413EC74BF2F5").IsUnique();

            entity.Property(e => e.Role)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusesAuction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statuses__3214EC07C210CF71");

            entity.ToTable("StatusesAuction");

            entity.HasIndex(e => e.Status, "UQ__Statuses__3A15923F06C7589A").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statuses__3214EC07863BB5EA");

            entity.ToTable("StatusesProduct");

            entity.HasIndex(e => e.Status, "UQ__Statuses__3A15923FDB6C7563").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TypesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypesPro__3214EC07A56E6F43");

            entity.ToTable("TypesProduct");

            entity.HasIndex(e => e.Type, "UQ__TypesPro__F9B8A48B607D3564").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC078BEFB1C1");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E429878931").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B88B9AD5").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__User_Add__3214EC07519C9CEB");

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
