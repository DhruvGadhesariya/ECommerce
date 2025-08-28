using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    /// <summary>
    /// Entity Framework Core database context for the E-Commerce system.
    /// Handles Users, Roles, and Addresses.
    /// </summary>
    public partial class UserHubDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public UserHubDbContext(DbContextOptions<UserHubDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // -------------------------
        // DbSets (tables)
        // -------------------------
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> Roles { get; set; }
        public virtual DbSet<UserAddress> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_configuration == null)
                    throw new InvalidOperationException("IConfiguration is not provided to the DbContext.");

                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -------------------------
            // User entity mapping
            // -------------------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK_tblUsers");

                entity.ToTable("tblUsers");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Avatar)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                // Relationships
                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK_tblUsers_Role");
            });

            // -------------------------
            // UserRole entity mapping
            // -------------------------
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK_tblUserRoles");

                entity.ToTable("tblUserRoles");

                entity.Property(e => e.RoleId).ValueGeneratedOnAdd();

                entity.Property(e => e.RoleName)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            });

            // -------------------------
            // UserAddress entity mapping
            // -------------------------
            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PK_tblUserAddress");

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

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                // Relationship: User → Addresses (1-to-many)
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblUserAddress_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
