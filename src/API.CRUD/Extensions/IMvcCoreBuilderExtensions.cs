using System;
using API.CRUD.ActionFilters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace API.CRUD.Extensions
{
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

        public static IMvcCoreBuilder AddCustomCors(this IMvcCoreBuilder mvcCoreBuilder,
                                                    string policyName)
        {
            if (mvcCoreBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcCoreBuilder));
            }

            if (string.IsNullOrWhiteSpace(policyName))
            {
                throw new ArgumentException(nameof(policyName));
            }

            // NOTE: Restrict the origin to any specific SPA url for public facing API's.
            return mvcCoreBuilder.AddCors(corsOptions =>
                                              corsOptions.AddPolicy(policyName,
                                                                    corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
                                                                                                          .AllowAnyHeader()
                                                                                                          .AllowAnyMethod()));
        }
    }
}