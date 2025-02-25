using Microsoft.EntityFrameworkCore;
using TreeApi.Models;

namespace TreeApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Journal> Journals { get; set; }

        // Конструктор для DI
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
                optionsBuilder.UseNpgsql("Host=roundhouse.proxy.rlwy.net;Port=15873;Database=railway;Username=postgres;Password=lUvaDDNjFgjUXEVJGArTQTUzfEoUrxqc");
            }
        }
    }
}