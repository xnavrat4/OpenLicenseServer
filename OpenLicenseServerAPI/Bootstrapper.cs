using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using OpenLicenseServerBL;
using Infrastructure.EFCore;
//using MySql.Data.MySqlClient;
using Npgsql;
using OpenLicenseServerDAL.Data;

namespace OpenLicenseServerAPI;

public class Bootstrapper
{
    private readonly string _connString;
    private readonly string _privateKey;
    
    public Bootstrapper(string connString, string privateKey)
    {
        _privateKey = privateKey;
        _connString = connString;
    }

    public void PrepareBuilder(ContainerBuilder builder)
    {
        builder.RegisterModule(new EFModule(_connString));
        builder.RegisterModule(new BLModule());
        builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

        builder.Register<DataCrypter>(ctx => new DataCrypter(_privateKey))
            .InstancePerLifetimeScope();
        
        builder.RegisterAutoMapper();
    }

    public void PrepareDatabase(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetService<OLSDbContext>();
        // context?.Database.EnsureCreated();
    }
}