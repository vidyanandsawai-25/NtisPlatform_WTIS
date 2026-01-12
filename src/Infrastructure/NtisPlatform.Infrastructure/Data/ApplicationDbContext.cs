using Microsoft.EntityFrameworkCore;
using NtisPlatform.Core.Entities;
using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Infrastructure.Data;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Authentication entities
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<LoginAttempt> LoginAttempts { get; set; } = null!;
    
    // Organization configuration
    public DbSet<Organization> Organizations { get; set; } = null!;
    public DbSet<OrganizationSetting> OrganizationSettings { get; set; } = null!;
    public DbSet<AuthProvider> AuthProviders { get; set; } = null!;
    public DbSet<FeatureFlag> FeatureFlags { get; set; } = null!;

    // PTIS entities
    public DbSet<ConstructionTypeEntity> ConstructionTypeEntity { get; set; } = null!;
    public DbSet<FloorEntity> FloorEntity { get; set; } = null!;
    public DbSet<SubFloorEntity> SubFloorEntity { get; set; } = null!;

    // WTIS Master entities
    public DbSet<ConnectionTypeMasterEntity> ConnectionTypeMaster { get; set; } = null!;
    public DbSet<ConnectionCategoryMasterEntity> ConnectionCategoryMaster { get; set; } = null!;
    public DbSet<PipeSizeMasterEntity> PipeSizeMaster { get; set; } = null!;
    public DbSet<ZoneMasterEntity> ZoneMaster { get; set; } = null!;
    public DbSet<WardMasterEntity> WardMaster { get; set; } = null!;
    public DbSet<RateMasterEntity> RateMaster { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // WTIS - Connection Type Master
        modelBuilder.Entity<ConnectionTypeMasterEntity>(entity =>
        {
            entity.ToTable("ConnectionTypeMaster", "WTIS");
            entity.HasKey(e => e.ConnectionTypeID);
            entity.Property(e => e.ConnectionTypeName).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.ConnectionTypeName).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired().HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.HasIndex(e => e.IsActive);
        });

        // WTIS - Connection Category Master
        modelBuilder.Entity<ConnectionCategoryMasterEntity>(entity =>
        {
            entity.ToTable("ConnectionCategoryMaster", "WTIS");
            entity.HasKey(e => e.CategoryID);
            entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.CategoryName).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired().HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.HasIndex(e => e.IsActive);
        });

        // WTIS - Pipe Size Master
        modelBuilder.Entity<PipeSizeMasterEntity>(entity =>
        {
            entity.ToTable("PipeSizeMaster", "WTIS");
            entity.HasKey(e => e.PipeSizeID);
            entity.Property(e => e.SizeName).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.SizeName).IsUnique();
            entity.Property(e => e.DiameterMM).IsRequired().HasColumnType("decimal(10,2)");
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired().HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.HasIndex(e => e.IsActive);
        });

        // WTIS - Zone Master
        modelBuilder.Entity<ZoneMasterEntity>(entity =>
        {
            entity.ToTable("ZoneMaster", "WTIS");
            entity.HasKey(e => e.ZoneID);
            entity.Property(e => e.ZoneName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ZoneCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.ZoneCode).IsUnique();
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired().HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.HasIndex(e => e.IsActive);
        });

        // WTIS - Ward Master
        modelBuilder.Entity<WardMasterEntity>(entity =>
        {
            entity.ToTable("WardMaster", "WTIS");
            entity.HasKey(e => e.WardID);
            entity.Property(e => e.WardName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.WardCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.WardCode).IsUnique();
            entity.Property(e => e.ZoneID).IsRequired();
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired().HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.HasIndex(e => e.ZoneID);
            entity.HasIndex(e => e.IsActive);
            
            entity.HasOne<ZoneMasterEntity>()
                .WithMany()
                .HasForeignKey(e => e.ZoneID)
                .HasConstraintName("FK_WardMaster_Zone")
                .OnDelete(DeleteBehavior.Restrict);
        });

        // WTIS - Rate Master
        modelBuilder.Entity<RateMasterEntity>(entity =>
        {
            entity.ToTable("RateMaster", "WTIS");
            entity.HasKey(e => e.RateID);
            
            entity.Property(e => e.ZoneID).IsRequired();
            entity.Property(e => e.WardID).IsRequired();
            entity.Property(e => e.TapSizeID).IsRequired();
            entity.Property(e => e.ConnectionTypeID).IsRequired();
            entity.Property(e => e.ConnectionCategoryID).IsRequired();
            entity.Property(e => e.MinReading).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.MaxReading).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PerLiter).IsRequired().HasColumnType("decimal(18,4)");
            entity.Property(e => e.MinimumCharge).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.MeterOffPenalty).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Rate).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Year).IsRequired();
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired().HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            
            entity.HasIndex(e => e.ZoneID).HasDatabaseName("IX_RateMaster_Zone");
            entity.HasIndex(e => e.WardID).HasDatabaseName("IX_RateMaster_Ward");
            entity.HasIndex(e => e.ConnectionTypeID).HasDatabaseName("IX_RateMaster_ConnectionType");
            entity.HasIndex(e => e.Year).HasDatabaseName("IX_RateMaster_Year");
            entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_RateMaster_IsActive");
            
            entity.HasOne<ZoneMasterEntity>()
                .WithMany()
                .HasForeignKey(e => e.ZoneID)
                .HasConstraintName("FK_RateMaster_Zone")
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne<WardMasterEntity>()
                .WithMany()
                .HasForeignKey(e => e.WardID)
                .HasConstraintName("FK_RateMaster_Ward")
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne<ConnectionTypeMasterEntity>()
                .WithMany()
                .HasForeignKey(e => e.ConnectionTypeID)
                .HasConstraintName("FK_RateMaster_ConnectionType")
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne<ConnectionCategoryMasterEntity>()
                .WithMany()
                .HasForeignKey(e => e.ConnectionCategoryID)
                .HasConstraintName("FK_RateMaster_ConnectionCategory")
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne<PipeSizeMasterEntity>()
                .WithMany()
                .HasForeignKey(e => e.TapSizeID)
                .HasConstraintName("FK_RateMaster_PipeSize")
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PTIS entities configuration
        modelBuilder.Entity<ConstructionTypeEntity>(entity =>
        {
            entity.ToTable("ConstructionTypeMaster", "PTIS");
            entity.HasKey(e => e.ConstructionId);
        });

        modelBuilder.Entity<FloorEntity>(entity =>
        {
            entity.ToTable("FloorMaster", "PTIS");
            entity.HasKey(e => e.FloorID);
        });

        modelBuilder.Entity<SubFloorEntity>(entity =>
        {
            entity.ToTable("SubFloorMaster", "PTIS");
            entity.HasKey(e => e.SubFloorId);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.TwoFactorSecret).HasMaxLength(200);
            
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Role configuration
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // UserRole configuration
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // RefreshToken configuration
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TokenHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.ClientType).HasMaxLength(50);
            entity.Property(e => e.DeviceInfo).HasMaxLength(1000);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.RevokedByIp).HasMaxLength(45);
            entity.Property(e => e.ReplacedByToken).HasMaxLength(500);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.TokenHash);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // LoginAttempt configuration
        modelBuilder.Entity<LoginAttempt>(entity =>
        {
            entity.ToTable("LoginAttempts");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IpAddress).IsRequired().HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.FailureReason).HasMaxLength(500);
            entity.Property(e => e.AuthProvider).HasMaxLength(50);
            entity.Property(e => e.ClientType).HasMaxLength(50);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.LoginAttempts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasIndex(e => e.AttemptedAt);
            entity.HasIndex(e => e.IpAddress);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Organization configuration (minimal entity)
        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organizations");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.IsSetupComplete).IsRequired();
            
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // OrganizationSetting configuration (key-value store)
        modelBuilder.Entity<OrganizationSetting>(entity =>
        {
            entity.ToTable("OrganizationSettings");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Value).HasColumnType("nvarchar(max)");
            entity.Property(e => e.DataType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            
            entity.HasIndex(e => e.Key).IsUnique();
            entity.HasIndex(e => e.Category);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // AuthProvider configuration
        modelBuilder.Entity<AuthProvider>(entity =>
        {
            entity.ToTable("AuthProviders");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProviderType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ConfigJson).HasColumnType("nvarchar(max)");
            
            entity.HasIndex(e => e.ProviderType);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // FeatureFlag configuration
        modelBuilder.Entity<FeatureFlag>(entity =>
        {
            entity.ToTable("FeatureFlags");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ModuleName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.MetadataJson).HasColumnType("nvarchar(max)");
            
            entity.HasIndex(e => e.ModuleName).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }
}
