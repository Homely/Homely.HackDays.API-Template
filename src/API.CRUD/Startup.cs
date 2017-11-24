using System;
using API.CRUD.Extensions;
using API.CRUD.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace API.CRUD
{
    public class Startup
    {
        private static string CorsPolicyName = "Default";
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration,
                       ILoggerFactory loggerFactory)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
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

            // RESTful framework.
            _logger.LogDebug("Adding common 'REST' Web Api services: Cors, Json responses, etc...");
            services.AddMvcCore()
                    .AddApiExplorer()  // for Swagger.
                    .AddDataAnnotations() // for validation.
                    .AddFormatterMappings()
                    .AddJsonFormatters(options => options = JsonHelpers.JsonSerializerSettings)
                    .AddCors(corsOptions => corsOptions.AddPolicy(CorsPolicyName,
                                                                  corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
                                                                                                        .AllowAnyHeader()
                                                                                                        .AllowAnyMethod()));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                                   new Info
                                   {
                                       Title = "Homely API Template",
                                       Version = "v1"
                                   });
            });

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
                app.JsonExceptionPage();
            }

            app.JsonStatusCodePages()
               .UseSwagger()
               .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Homely API Template v1"))
               .UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>()
               .UseMvc();
        }
    }
}
