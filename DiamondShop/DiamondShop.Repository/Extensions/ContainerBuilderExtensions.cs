using Autofac;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Repositories;
using System.Reflection;

namespace DiamondShop.Repository.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void AddRepositoriesDependency(this ContainerBuilder builder)
        {
            builder.RegisterRepositories();
        }

        private static void RegisterRepositories(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Repository")) 
                .AsImplementedInterfaces()                
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>))
                .InstancePerDependency();
            builder.RegisterGeneric(typeof(UnitOfWork<>))
                .As(typeof(IUnitOfWork<>)).InstancePerDependency();
            builder.RegisterType<Prn231DiamondShopContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}