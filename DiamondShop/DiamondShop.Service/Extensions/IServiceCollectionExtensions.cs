using DiamondShop.Repository.Models;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShop.Service.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices()
                    .AddMapsterConfigurations();
            return services;
        }

        private static IServiceCollection AddMapsterConfigurations(this IServiceCollection services)
        {
            services.AddMapster();
            return services;
        }


        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }

    }
}
