using Microsoft.EntityFrameworkCore;
using TreeApi.Models;

namespace TreeApi.Data
{
    public class AppDbContext : DbContext
    {
        // DbSet для узлов и журналов
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Journal> Journals { get; set; }

        // Конструктор для внедрения зависимостей (DI)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Защищённый конструктор для использования вне DI (например, в тестах)
        protected AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Строка подключения к PostgreSQL
                optionsBuilder.UseNpgsql("Host=roundhouse.proxy.rlwy.net;Port=15873;Database=railway;Username=postgres;Password=lUvaDDNjFgjUXEVJGArTQTUzfEoUrxqc");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка отношений между узлами
            modelBuilder.Entity<Node>()
                .HasOne(n => n.ParentNode) // Один узел имеет одного родителя
                .WithMany(n => n.Children) // Родитель может иметь много дочерних узлов
                .HasForeignKey(n => n.ParentNodeId) // Внешний ключ
                .OnDelete(DeleteBehavior.Restrict); // Запрет каскадного удаления
        }
    }
}