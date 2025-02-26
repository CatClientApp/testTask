using Microsoft.EntityFrameworkCore;
using TreeApi.Models;

namespace TreeApi.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Node> Nodes { get; set; }
        public DbSet<Journal> Journals { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Node>()
                .HasOne(n => n.ParentNode) 
                .WithMany(n => n.Children) 
                .HasForeignKey(n => n.ParentNodeId) 
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}