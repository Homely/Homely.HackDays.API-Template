using API.CRUD.ActionFilters;
using API.CRUD.Repositories;
using API.CRUD.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace API.CRUD
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(config => // basic API stuff
                    {
                        config.Filters.Add(new ValidateModelAttribute());
                    })
                    .AddApiExplorer() // for swagger
                    .AddFluentValidation(options => // for validation
                    {
                        options.RegisterValidatorsFromAssemblyContaining<Startup>();
                        options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    })
                    .AddJsonFormatters(); // for JSON responses

            services.AddSingleton<IPersonRepository, PersonRepository>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                                   new Info
                                   {
                                       Title = "Homely API Template",
                                       Version = "v1"
                                   });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages()
               .UseSwagger()
               .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Homely API Template v1"))
               .UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>()
               .UseMvc();
        }
    }
}