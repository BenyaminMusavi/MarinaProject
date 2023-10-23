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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MarinaDb2;integrated security=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.EmailAddress).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Province).HasMaxLength(50);
            entity.Property(e => e.Line).HasMaxLength(50);
            entity.Property(e => e.AgencyCode).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(false);
          //  entity.Property(e => e.IsAdmin).HasDefaultValue(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime2").HasDefaultValue(DateTime.Now);
            entity.Property(e => e.UpdaterUserId).IsRequired(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime2").IsRequired(false);
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}