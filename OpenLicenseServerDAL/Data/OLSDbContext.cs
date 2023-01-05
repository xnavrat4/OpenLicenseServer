using OpenLicenseServerDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenLicenseServerDAL.Models.HWIdentifiers;
using OperatingSystem = OpenLicenseServerDAL.Models.HWIdentifiers.OperatingSystem;


namespace OpenLicenseServerDAL.Data;

public class OLSDbContext : IdentityDbContext<IdentityUser>
{
    private string _connectionString;
    
    public DbSet<Device> Devices { get; set; }
    public DbSet<HWInfo> HwInfos { get; set; }
    
    //HW parts
    public DbSet<MotherBoard> MotherBoards { get; set; }
    public DbSet<OperatingSystem> OperatingSystems { get; set; }
    public DbSet<Processor> Processors { get; set; }
    public DbSet<RAMModule> RamModules { get; set; }
    public DbSet<MACAddress> MacAddresses { get; set; }
    public DbSet<Disk> Disks { get; set; }

    public DbSet<Violation> Violations { get; set; }
    public DbSet<ConnectionLog> ConnectionLogs { get; set; }
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<License> Licenses { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<User> Users { get; set; }

    public OLSDbContext()
    {
        _connectionString = "Host=localhost; Port=5432; Database=OLSdb; Username=postgres; Password=postgres";
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
            optionsBuilder
                .UseNpgsql(_connectionString)
                //.UseMySQL(_connectionString)
                .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>().Navigation(x => x.Customer).AutoInclude();
        modelBuilder.Entity<License>().Navigation(x => x.Device).AutoInclude();
        modelBuilder.Entity<HWInfo>()
            .Navigation(x => x.MotherBoard).AutoInclude();
        modelBuilder.Entity<HWInfo>()
            .Navigation(x => x.Processor).AutoInclude();
        modelBuilder.Entity<HWInfo>()
            .Navigation(x => x.RAMModuleList).AutoInclude();
        modelBuilder.Entity<HWInfo>()
            .Navigation(x => x.OperatingSystem).AutoInclude();
        modelBuilder.Entity<HWInfo>()
            .Navigation(x => x.DiskList).AutoInclude();
        modelBuilder.Entity<HWInfo>()
            .Navigation(x => x.MACAddressList).AutoInclude();
        
        
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }

        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}