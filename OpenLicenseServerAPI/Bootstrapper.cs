using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using OpenLicenseServerBL;
using Infrastructure.EFCore;
//using MySql.Data.MySqlClient;
using Npgsql;
using OpenLicenseServerDAL.Data;

namespace OpenLicenseServerAPI;

public class Bootstrapper : IDisposable
{
    //private readonly MySqlConnection _connection;
    private readonly NpgsqlConnection _connection;
    
    public Bootstrapper(string connString)
    {
        _connection = new NpgsqlConnection(connString);
        //_connection = new MySqlConnection(connString);
        _connection.Open();
    }

    public void PrepareBuilder(ContainerBuilder builder)
    {
        builder.RegisterModule(new EFModule(_connection));
        builder.RegisterModule(new BLModule());
        builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        
        builder.RegisterAutoMapper();
    }

    public void PrepareDatabase(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetService<OLSDbContext>();
        // context?.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}