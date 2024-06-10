using Carglass.TechnicalAssessment.Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carglass.TechnicalAssessment.Backend.DL.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ApplicationInMemoryDb");
    }
}