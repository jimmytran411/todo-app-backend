using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

#nullable disable

namespace TodoApp.Models
{
    public partial class TodoAppContext : DbContext
    {
        static TodoAppContext()
       => NpgsqlConnection.GlobalTypeMapper.MapEnum<StatusTypes>();

        public TodoAppContext()
        {
        }

        public TodoAppContext(DbContextOptions<TodoAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TodoItem> TodoItems { get; set; }
        public virtual DbSet<User> Users { get; set; }

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

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TodoItems)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("User_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
