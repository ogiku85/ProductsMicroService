using Microsoft.Extensions.DependencyInjection;
using ProductsMicroService.Repository.Repository;
using ProductsMicroService.Service.Factories;
using ProductsMicroService.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsMicroService.API.Extensions
{
    public static class RepositoryAndServiceRegistrationExtension
    {
        public static IServiceCollection AddRepositoriesAndServices(this IServiceCollection services)
        {

            //add custom services and repository
            services.AddScoped(typeof(GenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
            services.AddScoped(typeof(IGenericObjectFactory<,>), typeof(GenericObjectFactory<,>));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
