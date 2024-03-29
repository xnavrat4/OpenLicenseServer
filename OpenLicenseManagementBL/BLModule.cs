﻿using System.Reflection;
using Autofac;
using AutoMapper;
using OpenLicenseManagementBL.MappingProfile;
using OpenLicenseManagementBL.Services;
using Module = Autofac.Module;

namespace OpenLicenseManagementBL
{
    public class BLModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // QueryObjects
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Namespace == "OpenLicenseManagementBL.QueryObjects")
                .InstancePerDependency();
            //Services
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Namespace == "OpenLicenseManagementBL.Services")
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name)!)
                .InstancePerDependency();
            // Facades
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Namespace == "OpenLicenseManagementBL.Facades")
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name)!)
                .InstancePerDependency();
            
            // AutoMapper Profile
            builder.RegisterType<MappingProfile.MappingProfile>().As<Profile>().AutoActivate();
        }
    }
}
