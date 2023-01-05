using System.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using OpenLicenseServerDAL.Data;
using OpenLicenseServerDAL.Models;
using ConfigurationManager = System.Configuration.ConfigurationManager;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
            
var connectionString = config.GetRequiredSection("OLSConnectionString").Get<string>();


using (var context = new OLSDbContext(connectionString!)){
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    Console.WriteLine("Database created and seeded");
}
