using System.Configuration;
using OpenLicenseServerDAL.Data;
using OpenLicenseServerDAL.Models;

using (var context = new OLSDbContext(ConfigurationManager.AppSettings["OLSConnectionString"]))
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    foreach (var license in context.Licenses)
    {
        Console.WriteLine("License: " + license.SerialNumber);
    }
}
