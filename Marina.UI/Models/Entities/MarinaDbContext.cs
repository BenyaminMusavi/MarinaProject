using Microsoft.EntityFrameworkCore;

namespace Marina.UI.Models.Entities;

public partial class MarinaDbContext : DbContext
{
    public MarinaDbContext()
    {

    }
    public MarinaDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<RSM> RSMs { get; set; }
    public DbSet<Distributor> Distributors { get; set; }
    public DbSet<Line> Lines { get; set; }
    public DbSet<Province> Provinces { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
      //=> optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MarinaDb;integrated security=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Province>(entity =>
        {
            entity.ToTable("Province");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new Line { Id = 1, Name = "Teshran" }, new Line { Id = 2, Name = "Shiraz" });
        });

        modelBuilder.Entity<Line>(entity =>
        {
            entity.ToTable("Line");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new Line { Id = 1, Name = "SunStar" }, new Line { Id = 2, Name = "Sunnyness" });
        });

        modelBuilder.Entity<Distributor>(entity =>
        {
            entity.ToTable("Distributor");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).HasMaxLength(50);
        });

        modelBuilder.Entity<RSM>(entity =>
        {
            entity.ToTable("RSM");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new RSM { Id = 1, Name = "RSM1" }, new RSM { Id = 2, Name = "RSM2" }, new RSM { Id = 3, Name = "RSM3" });
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("Region");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new Region { Id = 1, Name = "R1" }, new Region { Id = 2, Name = "R2" }, new Region { Id = 3, Name = "R3" });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DistributorName).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50).IsUnicode();
            entity.Property(e => e.PhoneNumber).HasMaxLength(11).IsUnicode();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreateDate).HasColumnType("datetime2").HasDefaultValue(DateTime.Now);
            entity.Property(e => e.UpdaterUserId).IsRequired(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime2").IsRequired(false);

        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}