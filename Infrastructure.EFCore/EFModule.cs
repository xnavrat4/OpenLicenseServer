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
//using MySql.Data.MySqlClient;
using Npgsql;

namespace Infrastructure.EFCore
{
    public class EFModule : Module
    {
        private readonly string _connectionString;

        public EFModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var dbContextOptions = new DbContextOptionsBuilder<OLSDbContext>()
                .UseNpgsql(_connectionString)
                //.UseMySQL(_connection)
                .Options;

            // DB Context
            builder.Register<OLSDbContext>(ctx => new OLSDbContext(dbContextOptions))
                .InstancePerLifetimeScope();

            // Repositories
            builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IRepository<>));

            // Query
            builder.RegisterGeneric(typeof(EFQuery<>)).As(typeof(IQuery<>));

            // Unit of work
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        }
    }
}
