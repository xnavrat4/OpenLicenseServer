using System.Reflection;
using Autofac;
using AutoMapper;
using OpenLicenseServerBL.MappingProfile;
using OpenLicenseServerBL.Services;
using Module = Autofac.Module;

namespace OpenLicenseServerBL
{
    public class BLModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //Services
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IService<>));
            // Facades
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Namespace == "OpenLicenseServerBL.Facades")
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name)!)
                .InstancePerDependency();
            
            // AutoMapper Profile
            builder.RegisterType<MappingProfile.MappingProfile>().As<Profile>().AutoActivate();
        }
    }
}
