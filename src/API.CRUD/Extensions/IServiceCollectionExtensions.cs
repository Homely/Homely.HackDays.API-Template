using System;
using API.CRUD.ActionFilters;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace API.CRUD.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IMvcCoreBuilder AddCustomMvcCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddMvcCore(config => { config.Filters.Add(new ValidateModelAttribute()); });
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                                   new Info
                                   {
                                       Title = "Homely API Template",
                                       Version = "v1"
                                   });
            });
        }
    }
}