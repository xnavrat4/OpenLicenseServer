using Autofac;
using OpenLicenseServerDAL.Data;
using Infrastructure.EFCore.Query;
using Infrastructure.EFCore.Repository;
using Infrastructure.EFCore.UnitOfWork;
using Infrastructure.Query;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Infrastructure.EFCore
{
    public class EFModule : Module
    {
        private NpgsqlConnection _connection;
        //private MySqlConnection _connection;

        public EFModule(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var dbContextOptions = new DbContextOptionsBuilder<OLSDbContext>()
                // .UseNpgsql(_connection)
                .UseMySQL(_connection)
                .Options;

            // DB Context
            builder.Register<OLSDbContext>(ctx => new OLSDbContext(dbContextOptions))
                .InstancePerLifetimeScope()
                .OnActivated(e => Console.WriteLine($"Build {e.Instance.GetType().Name}"));

            // Repositories
            builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IRepository<>));

            // Query
            builder.RegisterGeneric(typeof(EFQuery<>)).As(typeof(IQuery<>));

            // Unit of work
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>();
        }
    }
}