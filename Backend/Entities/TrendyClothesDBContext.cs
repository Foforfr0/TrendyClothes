using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

public partial class TrendyClothesDBContext : DbContext {
    public TrendyClothesDBContext () {
    }

    public TrendyClothesDBContext (DbContextOptions<TrendyClothesDBContext> options)
        : base (options) {
    }

    public virtual DbSet<AuctionsProduct> AuctionsProducts {
        get; set;
    }

    public virtual DbSet<CategoriesProduct> CategoriesProducts {
        get; set;
    }

    public virtual DbSet<PhotosProduct> PhotosProducts {
        get; set;
    }

    public virtual DbSet<Product> Products {
        get; set;
    }

    public virtual DbSet<QAProduct> QAProducts {
        get; set;
    }

    public virtual DbSet<RolesUser> RolesUsers {
        get; set;
    }

    public virtual DbSet<StatusesAuction> StatusesAuctions {
        get; set;
    }

    public virtual DbSet<StatusesProduct> StatusesProducts {
        get; set;
    }

    public virtual DbSet<TypesProduct> TypesProducts {
        get; set;
    }

    public virtual DbSet<User> Users {
        get; set;
    }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            var config = new ConfigurationBuilder ()
                .AddJsonFile ("appsettings.json")
                .Build ();

            var connectionString = config.GetConnectionString ("DefaultConnection");

            optionsBuilder.UseSqlServer (connectionString);
        }
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder) {
        modelBuilder.Entity<AuctionsProduct> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__Auctions__3214EC077549CD88");

            entity.ToTable ("AuctionsProduct");

            entity.Property (e => e.FirstPrice).HasColumnType ("decimal(12, 2)");
            entity.Property (e => e.LastPrice).HasColumnType ("decimal(12, 2)");
            entity.Property (e => e.MinBid).HasColumnType ("decimal(12, 2)");

            entity.HasOne (d => d.Buyer).WithMany (p => p.AuctionsProducts)
                .HasForeignKey (d => d.BuyerId)
                .HasConstraintName ("FK_BuyerProduct_Product");

            entity.HasOne (d => d.Product).WithMany (p => p.AuctionsProducts)
                .HasForeignKey (d => d.ProductId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_AuctionProduct_Product");

            entity.HasOne (d => d.Status).WithMany (p => p.AuctionsProducts)
                .HasForeignKey (d => d.StatusId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_StatusAuction_Auction");
        });

        modelBuilder.Entity<CategoriesProduct> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__Categori__3214EC078AEE71AD");

            entity.ToTable ("CategoriesProduct");

            entity.HasIndex (e => e.Category, "UQ__Categori__4BB73C322E49CDD6").IsUnique ();

            entity.Property (e => e.Category)
                .HasMaxLength (50)
                .IsUnicode (false);
        });

        modelBuilder.Entity<PhotosProduct> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__PhotosPr__3214EC07B0248907");

            entity.ToTable ("PhotosProduct");

            entity.HasOne (d => d.Product).WithMany (p => p.PhotosProducts)
                .HasForeignKey (d => d.ProductId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_PhotoProduct_Product");
        });

        modelBuilder.Entity<Product> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__Products__3214EC07FFF6675D");

            entity.Property (e => e.AverageStars).HasColumnType ("decimal(2, 1)");
            entity.Property (e => e.Description).HasColumnType ("text");
            entity.Property (e => e.Discount).HasColumnType ("decimal(12, 2)");
            entity.Property (e => e.Name)
                .HasMaxLength (100)
                .IsUnicode (false);
            entity.Property (e => e.Price).HasColumnType ("decimal(12, 2)");

            entity.HasOne (d => d.Category).WithMany (p => p.Products)
                .HasForeignKey (d => d.CategoryId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_CategoryProduct_Product");

            entity.HasOne (d => d.Seller).WithMany (p => p.Products)
                .HasForeignKey (d => d.SellerId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_Seller_Product");

            entity.HasOne (d => d.Status).WithMany (p => p.Products)
                .HasForeignKey (d => d.StatusId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_StatusProduct_Product");

            entity.HasOne (d => d.Type).WithMany (p => p.Products)
                .HasForeignKey (d => d.TypeId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_TypeProduct_Product");
        });

        modelBuilder.Entity<QAProduct> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__QAProduc__3214EC07656203DB");

            entity.ToTable ("QAProduct");

            entity.Property (e => e.Description).HasColumnType ("text");
            entity.Property (e => e.Stars).HasColumnType ("decimal(2, 1)");

            entity.HasOne (d => d.Product).WithMany (p => p.QAProducts)
                .HasForeignKey (d => d.ProductId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_QAProduct_Product");

            entity.HasOne (d => d.User).WithMany (p => p.QAProducts)
                .HasForeignKey (d => d.UserId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_UserQA_Product");
        });

        modelBuilder.Entity<RolesUser> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__RolesUse__3214EC07B78C3BEB");

            entity.ToTable ("RolesUser");

            entity.HasIndex (e => e.Role, "UQ__RolesUse__DA15413E83B8D7BA").IsUnique ();

            entity.Property (e => e.Role)
                .HasMaxLength (25)
                .IsUnicode (false);
        });

        modelBuilder.Entity<StatusesAuction> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__Statuses__3214EC07232073F0");

            entity.ToTable ("StatusesAuction");

            entity.HasIndex (e => e.Status, "UQ__Statuses__3A15923FAEF8CD3F").IsUnique ();

            entity.Property (e => e.Status)
                .HasMaxLength (20)
                .IsUnicode (false);
        });

        modelBuilder.Entity<StatusesProduct> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__Statuses__3214EC073E730143");

            entity.ToTable ("StatusesProduct");

            entity.HasIndex (e => e.Status, "UQ__Statuses__3A15923FDE286DAA").IsUnique ();

            entity.Property (e => e.Status)
                .HasMaxLength (20)
                .IsUnicode (false);
        });

        modelBuilder.Entity<TypesProduct> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__TypesPro__3214EC07A36FE620");

            entity.ToTable ("TypesProduct");

            entity.HasIndex (e => e.Type, "UQ__TypesPro__F9B8A48BD9D4D0B6").IsUnique ();

            entity.Property (e => e.Type)
                .HasMaxLength (20)
                .IsUnicode (false);
        });

        modelBuilder.Entity<User> (entity => {
            entity.HasKey (e => e.Id).HasName ("PK__Users__3214EC07C43A3745");

            entity.HasIndex (e => e.Username, "UQ__Users__536C85E4FDAB8E06").IsUnique ();

            entity.HasIndex (e => e.Email, "UQ__Users__A9D10534613C8B4A").IsUnique ();

            entity.Property (e => e.Email)
                .HasMaxLength (100)
                .IsUnicode (false);
            entity.Property (e => e.FirstName)
                .HasMaxLength (50)
                .IsUnicode (false);
            entity.Property (e => e.LastName)
                .HasMaxLength (50)
                .IsUnicode (false);
            entity.Property (e => e.MiddleName)
                .HasMaxLength (50)
                .IsUnicode (false);
            entity.Property (e => e.Password).HasMaxLength (200);
            entity.Property (e => e.Username)
                .HasMaxLength (40)
                .IsUnicode (false);

            entity.HasOne (d => d.Role).WithMany (p => p.Users)
                .HasForeignKey (d => d.RoleId)
                .OnDelete (DeleteBehavior.ClientSetNull)
                .HasConstraintName ("FK_RoleUser_User");
        });

        OnModelCreatingPartial (modelBuilder);
    }

    partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
}
