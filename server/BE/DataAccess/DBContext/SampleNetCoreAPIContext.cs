using DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContext
{
  public class SampleNetCoreAPIContext : DbContext
  {
    public SampleNetCoreAPIContext(DbContextOptions<SampleNetCoreAPIContext> options)
: base(options)
    { }

    public virtual DbSet<Menu> Menus { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<HotelManagementCoreConfig> HotelManagementCoreConfig { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<HotelManagementCoreConfig>(entity =>
      {
        entity.ToTable("hotel_management_net_core_config");
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.ToTable("users");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");
      });

      modelBuilder.Entity<Menu>(entity =>
      {
        entity.ToTable("menus");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");
      });

      modelBuilder.Entity<Blog>(entity =>
      {
        entity.ToTable("blogs");

        entity.Property(e => e.Id).HasColumnType("int");
      });

      modelBuilder.Entity<Post>(entity =>
      {
        entity.ToTable("posts");

        entity.HasIndex(e => e.BlogId)
                  .HasName("FK_Post_Blog_BlogId_idx");

        entity.HasOne(d => d.Blog)
                  .WithMany(p => p.Posts)
                  .HasForeignKey(d => d.BlogId)
                  .HasConstraintName("FK_Post_Blog_BlogId");
      });
    }
  }
}
