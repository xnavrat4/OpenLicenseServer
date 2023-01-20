using OpenLicenseServerDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace OpenLicenseServerDAL.Data;

public class OLSDbContext : IdentityDbContext<IdentityUser>
{
    private string _connectionString;
    
    public DbSet<Device> Devices { get; set; }
    public DbSet<HWInfo> HwInfos { get; set; }
    public DbSet<License> Licenses { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public OLSDbContext()
    {
        _connectionString = "const";
    }
    
    public OLSDbContext(DbContextOptions options): base(options)
    {
    }

    public OLSDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
                //.UseNpgsql(_connectionString)
            optionsBuilder
                .UseMySQL(_connectionString)
                .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}