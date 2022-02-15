using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

#nullable disable

namespace TodoApp.Models
{
    public partial class TodoAppContext : IdentityDbContext<ApplicationUser>
    {
        static TodoAppContext()
       => NpgsqlConnection.GlobalTypeMapper.MapEnum<StatusTypes>();

        public TodoAppContext(DbContextOptions<TodoAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                 optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<StatusTypes>();

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.TodoItems)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("User_id");
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
