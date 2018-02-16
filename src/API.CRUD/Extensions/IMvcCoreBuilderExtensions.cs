using API.CRUD.ActionFilters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace API.CRUD.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IMvcCoreBuilderExtensions
    {
        public static IMvcCoreBuilder AddCustomFluentValidation(this IMvcCoreBuilder mvcCoreBuilder)
        {
            if (mvcCoreBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcCoreBuilder));
            }

            return mvcCoreBuilder.AddFluentValidation(options =>
                                 {
                                     options.RegisterValidatorsFromAssemblyContaining<Startup>();
                                     options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                                 })
                                 .AddMvcOptions(options => options.Filters.Add(new ValidateModelAttribute()));
        }

        public static IMvcCoreBuilder AddCustomJsonFormatters(this IMvcCoreBuilder mvcCoreBuilder)
        {
            return mvcCoreBuilder.AddJsonFormatters(options =>
            {
                options.NullValueHandling = NullValueHandling.Ignore;
                options.Formatting = Formatting.Indented;
                options.Converters.Add(new StringEnumConverter());
            });
        }
    }
}