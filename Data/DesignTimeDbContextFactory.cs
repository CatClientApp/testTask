using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TreeApi.Data;

namespace TreeApi.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=roundhouse.proxy.rlwy.net;Port=15873;Database=railway;Username=postgres;Password=lUvaDDNjFgjUXEVJGArTQTUzfEoUrxqc");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}