using Microsoft.EntityFrameworkCore;

namespace mf_api_web_services_fuel_manager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Veiculo> veiculos { get; set; } = null!;
    }
}