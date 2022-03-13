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
        public virtual DbSet<Category> Categories { get; set; }
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
                optionsBuilder.UseSqlServer("Server=DESKTOP-O05LV9C\\SQLEXPRESS;Database=delegator;Trusted_Connection=True;");
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

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .IsFixedLength(true);

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.Color).IsRequired();
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
                entity.ToTable("CompanyUser");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .IsFixedLength(true);

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

                entity.HasOne(d => d.AppUser);

                entity.HasOne(d => d.Company);

                entity.HasOne(d => d.Role);
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

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("CategoryID")
                    .IsFixedLength(true);

                entity.Property(e => e.EndTime).HasColumnType("date");

                entity.Property(e => e.SenderId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("SenderID")
                    .IsFixedLength(true);

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.Category);

                entity.HasOne(d => d.Sender);
            });

            modelBuilder.Entity<TasksTask>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .IsFixedLength(true);

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

                entity.HasOne(d => d.MainTask);

                entity.HasOne(d => d.Task);
            });

            modelBuilder.Entity<TasksUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .IsFixedLength(true);

                entity.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("CompanyID")
                    .IsFixedLength(true);

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

                entity.HasOne(d => d.Company);

                entity.HasOne(d => d.Task);

                entity.HasOne(d => d.User);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
