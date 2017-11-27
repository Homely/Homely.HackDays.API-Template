using System;
using API.CRUD.Extensions;
using API.CRUD.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackifyMiddleware;

namespace API.CRUD
{
    public class Startup
    {
        private const string CorsPolicyName = "Default";
        private readonly ILogger _logger;
            
        public Startup(IConfiguration configuration,
                       ILoggerFactory loggerFactory)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            _logger = loggerFactory.CreateLogger<Startup>();

            _logger.LogInformation($" <::::[]==0 ------------------------------------------ o==[]::::> {Environment.NewLine} *** Starting WebApi.");
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            _logger.LogDebug("Adding common 'REST' Web Api services: Cors, Json responses, etc...");

            services.AddMvcCore()
                    .AddCustomJsonFormatters()
                    .AddApiExplorer() // for Swagger.
                    .AddCustomFluentValidation()
                    .AddFormatterMappings() // TODO: Check what this does.
                    .AddCustomCors(CorsPolicyName);

            services.AddSwagger();

            services.AddSingleton<IPersonRepository, PersonRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.JsonExceptionPage(CorsPolicyName);
            }

            app.JsonStatusCodePages()
               .UseSwagger()
               .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Homely API Template v1"))
               .UseMiddleware<RequestTracerMiddleware>()
               .UseMvc();
        }
    }
}