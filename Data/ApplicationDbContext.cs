using KAS.CMS.Models;
using Microsoft.EntityFrameworkCore;

namespace KAS.CMS.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Define your DbSet properties for the form data models
    // For example:
    public DbSet<DocumentModel> Documents { get; set; }
    public DbSet<MineTypeModel> MineTypes { get; set; }
    public DbSet<CityModel> Cities { get; set; }
    public DbSet<DocumentTypeModel> DocumentTypes { get; set; }
    public DbSet<StartYearModel> StartYears { get; set; }
    public DbSet<EndYearModel> EndYears { get; set; }
    
}