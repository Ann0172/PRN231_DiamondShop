using Autofac;
using Autofac.Extensions.DependencyInjection;
using DiamondShop.Service.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Google.Cloud.Storage.V1;

namespace DiamondShop.Service.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void AddServicesDependency(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterServices();
            builder.RegisterMapster();
        }

        private static void RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Register(ctx =>
            {
                var storageClient = StorageClient.Create();
                return storageClient;
            }).SingleInstance();
        }
        

        private static void RegisterMapster(this ContainerBuilder builder)
        {
            var config = new TypeAdapterConfig();
            config.Scan(Assembly.GetExecutingAssembly());
            
            builder.RegisterInstance(config).AsSelf().SingleInstance();
            builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();
        }
    }
}