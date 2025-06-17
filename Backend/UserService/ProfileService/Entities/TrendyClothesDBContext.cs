using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProfileService.Entities;

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
            string? environment = Environment.GetEnvironmentVariable ("ASPNETCORE_ENVIRONMENT") ?? "Production";

            IConfigurationRoot? config = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json", optional: false)
                .AddJsonFile ($"appsettings.{environment}.json", optional: true)
                .Build ();

            string? connectionString = config.GetConnectionString ("SQLServer");

            optionsBuilder.UseSqlServer (connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC07597F568F");

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
            entity.HasKey(e => e.Id).HasName("PK__Auctions__3214EC0715A37928");

            entity.ToTable("AuctionsProduct");

            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.FirstPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.LastPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.MinBid).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.AuctionsProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuctionProduct_Product");

            entity.HasOne(d => d.Seller).WithMany(p => p.AuctionsProducts)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BuyerProduct_Product");

            entity.HasOne(d => d.Status).WithMany(p => p.AuctionsProducts)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusAuction_Auction");
        });

        modelBuilder.Entity<BidsAuction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BidsAuct__3214EC07FA07A44A");

            entity.ToTable("BidsAuction");

            entity.Property(e => e.Bid).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.DateBid).HasColumnType("datetime");

            entity.HasOne(d => d.Auction).WithMany(p => p.BidsAuctions)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BidAuction_Auction");

            entity.HasOne(d => d.Buyer).WithMany(p => p.BidsAuctions)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BidUser_Auction");
        });

        modelBuilder.Entity<CategoriesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0751992404");

            entity.ToTable("CategoriesProduct");

            entity.HasIndex(e => e.Category, "UQ__Categori__4BB73C32DC13E7E8").IsUnique();

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhotosProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotosPr__3214EC073478280F");

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
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07C20672D9");

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
            entity.HasKey(e => e.Id).HasName("PK__QAProduc__3214EC0738D89B90");

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
            entity.HasKey(e => e.Id).HasName("PK__RolesUse__3214EC07D353D560");

            entity.ToTable("RolesUser");

            entity.HasIndex(e => e.Role, "UQ__RolesUse__DA15413EDCB4B961").IsUnique();

            entity.Property(e => e.Role)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusesAuction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statuses__3214EC07BE43C483");

            entity.ToTable("StatusesAuction");

            entity.HasIndex(e => e.Status, "UQ__Statuses__3A15923F9981329C").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statuses__3214EC0790488393");

            entity.ToTable("StatusesProduct");

            entity.HasIndex(e => e.Status, "UQ__Statuses__3A15923FBECB2422").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TypesProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypesPro__3214EC07C2FDF014");

            entity.ToTable("TypesProduct");

            entity.HasIndex(e => e.Type, "UQ__TypesPro__F9B8A48B429A74C6").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0710DDBDF7");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E417399129").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534751E01BA").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__User_Add__3214EC078EE76B67");

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
