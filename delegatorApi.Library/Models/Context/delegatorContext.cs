using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TasksTask> TasksTasks { get; set; }
        public virtual DbSet<TasksUser> TasksUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-O05LV9C\\SQLEXPRESS;Initial Catalog=delegator;Integrated Security=True;");
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

                entity.Property(e => e.Code).IsRequired();

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

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .IsFixedLength(true);

                entity.Property(e => e.EndTime).HasColumnType("date");

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<TasksTask>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.MainTaskId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("MainTaskID")
                    .IsFixedLength(true);

                entity.Property(e => e.TaskId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("TaskID")
                    .IsFixedLength(true);

                entity.HasOne(d => d.MainTask)
                    .WithMany()
                    .HasForeignKey(d => d.MainTaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TasksTasks_Tasks");

                entity.HasOne(d => d.Task)
                    .WithMany()
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TasksTasks_Tasks1");
            });

            modelBuilder.Entity<TasksUser>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.TaskId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("TaskID")
                    .IsFixedLength(true);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("UserID")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Task)
                    .WithMany()
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TasksUsers_Tasks");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TasksUsers_AppUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
