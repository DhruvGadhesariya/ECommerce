using System;
using System.Collections.Generic;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data;

public partial class EcommercedbContext : DbContext
{
    public EcommercedbContext()
    {
    }

    public EcommercedbContext(DbContextOptions<EcommercedbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblApprovalStatus> TblApprovalStatuses { get; set; }

    public virtual DbSet<TblCart> TblCarts { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblCity> TblCities { get; set; }

    public virtual DbSet<TblCompany> TblCompanies { get; set; }

    public virtual DbSet<TblCountry> TblCountries { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblPaymentMethod> TblPaymentMethods { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblProductMedia> TblProductMedias { get; set; }

    public virtual DbSet<TblProductRating> TblProductRatings { get; set; }

    public virtual DbSet<TblProductReview> TblProductReviews { get; set; }

    public virtual DbSet<TblReviewMedia> TblReviewMedias { get; set; }

    public virtual DbSet<TblSubCategory> TblSubCategories { get; set; }

    public virtual DbSet<TblSupplier> TblSuppliers { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserAddress> TblUserAddresses { get; set; }

    public virtual DbSet<TblUserRole> TblUserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PCA118\\SQL2017;DataBase=ECOMMERCEDb;User ID=sa;Password=Tatva@123;encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblApprovalStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__tblAppro__C8EE20632F3445A7");

            entity.ToTable("tblApprovalStatus");

            entity.Property(e => e.StatusId).ValueGeneratedOnAdd();
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblCart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__tblCarts__51BCD7B7A89EACAF");

            entity.ToTable("tblCarts");

            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.TblCarts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__tblCarts__Produc__282DF8C2");

            entity.HasOne(d => d.User).WithMany(p => p.TblCarts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblCarts__UserId__2739D489");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tblCateg__19093A0B868F235F");

            entity.ToTable("tblCategories");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCity>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__tblCitie__F2D21B76E08BA0D1");

            entity.ToTable("tblCities");

            entity.Property(e => e.CityName)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Country).WithMany(p => p.TblCities)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__tblCities__Count__4CA06362");
        });

        modelBuilder.Entity<TblCompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__tblCompa__2D971CAC4A0DCCC7");

            entity.ToTable("tblCompanies");

            entity.Property(e => e.CompanyAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Country).WithMany(p => p.TblCompanies)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__tblCompan__Count__6B24EA82");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.TblCompanies)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("FK__tblCompan__Statu__6C190EBB");
        });

        modelBuilder.Entity<TblCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__tblCount__10D1609F933E65F9");

            entity.ToTable("tblCountries");

            entity.Property(e => e.CountryId).ValueGeneratedOnAdd();
            entity.Property(e => e.CountryName)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__tblOrder__C3905BCF520446CF");

            entity.ToTable("tblOrders");

            entity.Property(e => e.CancledAt).HasColumnType("datetime");
            entity.Property(e => e.OrderAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.PaymentMethodNavigation).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.PaymentMethod)
                .HasConstraintName("FK__tblOrders__Payme__2EDAF651");

            entity.HasOne(d => d.Product).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__tblOrders__Produ__2BFE89A6");

            entity.HasOne(d => d.UserAddressNavigation).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.UserAddress)
                .HasConstraintName("FK__tblOrders__UserA__2FCF1A8A");

            entity.HasOne(d => d.User).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblOrders__UserI__2CF2ADDF");
        });

        modelBuilder.Entity<TblPaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("PK__tblPayme__FC681851399A4B07");

            entity.ToTable("tblPaymentMethods");

            entity.Property(e => e.MethodId).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.MethodName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__tblProdu__B40CC6CD855F7989");

            entity.ToTable("tblProducts");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Discount)
                .HasDefaultValueSql("((0))")
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IsFree).HasDefaultValueSql("((0))");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.StokeKeepingUnit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tblProduc__Categ__08B54D69");

            entity.HasOne(d => d.Company).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__tblProduc__Compa__0B91BA14");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("FK__tblProduc__Statu__07C12930");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.SubCategoryId)
                .HasConstraintName("FK__tblProduc__SubCa__09A971A2");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__tblProduc__Suppl__0A9D95DB");
        });

        modelBuilder.Entity<TblProductMedia>(entity =>
        {
            entity.HasKey(e => e.MediaId).HasName("PK__tblProdu__B2C2B5CF0DCA2171");

            entity.ToTable("tblProductMedias");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Default).HasDefaultValueSql("((0))");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.MediaName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MediaPath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MediaType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.TblProductMedia)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__tblProduc__Produ__114A936A");
        });

        modelBuilder.Entity<TblProductRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__tblProdu__FCCDF87CDADC9FE8");

            entity.ToTable("tblProductRatings");

            entity.Property(e => e.RatingId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Rating).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.TblProductRatings)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__tblProduc__Produ__19DFD96B");

            entity.HasOne(d => d.User).WithMany(p => p.TblProductRatings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblProduc__UserI__18EBB532");
        });

        modelBuilder.Entity<TblProductReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__tblProdu__74BC79CE7E9AC1C4");

            entity.ToTable("tblProductReviews");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.TblProductReviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__tblProduc__Produ__1EA48E88");

            entity.HasOne(d => d.User).WithMany(p => p.TblProductReviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblProduc__UserI__1DB06A4F");
        });

        modelBuilder.Entity<TblReviewMedia>(entity =>
        {
            entity.HasKey(e => e.ReviewMediaId).HasName("PK__tblRevie__5C018B1CFB7C9673");

            entity.ToTable("tblReviewMedias");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.ReviewMediaName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ReviewMediaPath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ReviewMediaType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Review).WithMany(p => p.TblReviewMedia)
                .HasForeignKey(d => d.ReviewId)
                .HasConstraintName("FK__tblReview__Revie__22751F6C");
        });

        modelBuilder.Entity<TblSubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("PK__tblSubCa__26BE5B1933361395");

            entity.ToTable("tblSubCategories");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.SubCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.TblSubCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tblSubCat__Categ__5FB337D6");
        });

        modelBuilder.Entity<TblSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__tblSuppl__4BE666B4FA229BA8");

            entity.ToTable("tblSuppliers");

            entity.Property(e => e.ContactNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.TblSuppliers)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__tblSuppli__Compa__607251E5");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.TblSuppliers)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("FK__tblSuppli__Statu__71D1E811");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tblUsers__1788CC4C37C290DB");

            entity.ToTable("tblUsers");

            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Firstname)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Lastname)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasDefaultValueSql("((1))");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__tblUsers__CityId__5812160E");

            entity.HasOne(d => d.Country).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__tblUsers__Countr__571DF1D5");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("FK__tblUsers__Role__5441852A");
        });

        modelBuilder.Entity<TblUserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__tblUserA__091C2AFB9CA9230F");

            entity.ToTable("tblUserAddress");

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.TblUserAddresses)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__tblUserAd__CityI__778AC167");

            entity.HasOne(d => d.Country).WithMany(p => p.TblUserAddresses)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__tblUserAd__Count__76969D2E");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tblUserAd__UserI__75A278F5");
        });

        modelBuilder.Entity<TblUserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__tblUserR__8AFACE1AC769C271");

            entity.ToTable("tblUserRoles");

            entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.RoleName)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
