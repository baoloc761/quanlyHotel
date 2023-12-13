using DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContext
{
  public class SampleNetCoreAPIContext : DbContext
  {
    public SampleNetCoreAPIContext(DbContextOptions<SampleNetCoreAPIContext> options)
: base(options)
    { }

    public virtual DbSet<AttachmentGroup> AttachmentGroups { get; set; }
    public virtual DbSet<Attachment> Attachments { get; set; }
    public virtual DbSet<UserAttachment> UserAttachments { get; set; }
    public virtual DbSet<Menu> Menus { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserType> UserTypes { get; set; }
    public virtual DbSet<UserTypeUser> UserTypeUsers { get; set; }
    public virtual DbSet<UsersMenusPermission> UsersMenusPermissions { get; set; }
    public virtual DbSet<HotelManagementCoreConfig> HotelManagementCoreConfig { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<HotelManagementCoreConfig>(entity =>
      {
        entity.ToTable("hotel_management_net_core_config");
      });

      modelBuilder.Entity<AttachmentGroup>(entity =>
      {
        entity.ToTable("attachment_groups");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");
      });

      modelBuilder.Entity<Attachment>(entity =>
      {
        entity.ToTable("attachments");

        entity.HasIndex(e => e.FileGroupId)
                  .HasName("IX_attachments_FileGroupId");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");

        entity.HasOne(d => d.AttachmentGroup)
              .WithOne(p => p.Attachment)
              .HasForeignKey<Attachment>(d => d.FileGroupId)
              .HasConstraintName("FK_attachments_attachment_groups_FileGroupId")
              .IsRequired();
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.ToTable("users");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");
      });

      modelBuilder.Entity<UserAttachment>(entity =>
      {
        entity.ToTable("user_attachments");

        entity.HasIndex(e => e.UserId)
                  .HasName("FK_user_attachments_users_UserId");

        entity.HasIndex(e => e.FileId)
                  .HasName("FK_user_attachments_attachemnts_FileId");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");

      });

      modelBuilder.Entity<UserType>(entity =>
      {
        entity.ToTable("user_types");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");
      });

      modelBuilder.Entity<UserTypeUser>(entity =>
      {
        entity.ToTable("user_type_users");

        entity.HasIndex(e => e.UserId)
                  .HasName("IX_user_type_users_UserId");

        entity.HasIndex(e => e.UserTypeId)
                  .HasName("IX_user_type_users_UserTypeId");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");

        entity.HasOne(d => d.User)
                .WithMany(p => p.UserTypeUser)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_user_type_users_users_UserId");

        entity.HasOne(d => d.UserType)
                .WithMany(p => p.UserTypeUser)
                  .HasForeignKey(d => d.UserTypeId)
                  .HasConstraintName("FK_user_type_users_user_types_UserTypeId");
      });


      modelBuilder.Entity<Menu>(entity =>
      {
        entity.ToTable("menus");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");
      });

      modelBuilder.Entity<UsersMenusPermission>(entity =>
      {
        entity.ToTable("users_menus_permissions");

        entity.HasIndex(e => e.UserId)
                  .HasName("IX_users_menus_permissions_UserId");

        entity.HasIndex(e => e.MenuId)
                  .HasName("IX_users_menus_permissions_MenuId");

        entity.Property(e => e.Id).HasColumnType("UNIQUEIDENTIFIER");
      });
    }
  }
}
