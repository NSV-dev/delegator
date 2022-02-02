using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using delegatorApi.Library.Models;

#nullable disable

namespace delegatorApi.Library.Models.Context
{
    public partial class delegatorContext : DbContext
    {
        public delegatorContext()
        {
        }

        public delegatorContext(DbContextOptions<delegatorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyUser> CompanyUsers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-O05LV9C\\SQLEXPRESS;Initial Catalog=delegator;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .IsFixedLength(true);

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.UserName).IsRequired();
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .IsFixedLength(true);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<CompanyUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CompanyUser");

                entity.Property(e => e.AppUserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsFixedLength(true);

                entity.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsFixedLength(true);

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsFixedLength(true);

                entity.HasOne(d => d.AppUser)
                    .WithMany()
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyUser_AppUsers");

                entity.HasOne(d => d.Company)
                    .WithMany()
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyUser_Companies");

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyUser_Roles");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .IsFixedLength(true);

                entity.Property(e => e.Title).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
