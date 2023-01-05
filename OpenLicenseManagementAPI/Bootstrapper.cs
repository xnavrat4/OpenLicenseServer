using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Infrastructure.EFCore;
using MySql.Data.MySqlClient;
using Npgsql;
using OpenLicenseManagementBL;
using OpenLicenseServerDAL.Data;

namespace OpenLicenseManagementAPI;

public class Bootstrapper
{
    private readonly string _connString;

    public Bootstrapper(string connString)
    {
        _connString = connString;
    }

    public void PrepareBuilder(ContainerBuilder builder)
    {
        builder.RegisterModule(new EFModule(_connString));
        builder.RegisterModule(new BLModule());
        builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

        builder.RegisterAutoMapper();
    }

    public void PrepareDatabase(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetService<OLSDbContext>();
        // context?.Database.EnsureCreated();
    }
}
