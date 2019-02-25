using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjectParking.Contracts;
using ProjectParking.WebApps.ParkingAPI.Entities;
using ProjectParking.WebApps.ParkingAPI.Utilities;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using ProjectParking.WebApps.ParkingAPI.Middlewares;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ProjectParking.WebApps.ParkingAPI.Extensions;
using ProjectParking.WebApps.ParkingAPI.Providers.Contracts;
using ProjectParking.WebApps.ParkingAPI.Providers.InMemory;
using ProjectParking.WebApps.ParkingAPI.Hubs;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using ProjectParking.WebApps.ParkingAPI.BackgroundTasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SignalR;
using ProjectParking.WebApps.ParkingAPI.Consumers;
using ProjectParking.WebApps.ParkingAPI.Managers;

namespace ProjectParking.WebApps.ParkingAPI
{
    public class Startup
    {
        private IContainer container;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AuthResponsesOperationFilter>();
                options.SwaggerDoc("v1", new Info { Title = "Parking API", Version = "v1" });
                options.AddSecurityDefinition("API KEY", new ApiKeyScheme()
                {
                    Description = "Authorization header using your API Key",
                    Name = AppConstants.API_KEY_HEADER,
                    In = "header",
                });

                options.AddSecurityRequirement(
                    new Dictionary<string, IEnumerable<string>>() { { "API KEY", new List<string>() } }
                    );

                var filePath = Path.Combine(AppContext.BaseDirectory, "ProjectParking.WebApps.ParkingAPI.xml");
                options.IncludeXmlComments(filePath);
            });

            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

            services.AddTransient<ICarparkProvider, InMemoryCarparkProvider>();
            services.AddTransient<IStartupFilter, CarparkSetupStartFilter>();
            services.AddTransient<CarparkManager, CarparkManager>();
            services.AddTransient<CarparkStatusManager, CarparkStatusManager>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddApplicationInsightsTelemetry(Configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                                           .ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddSingleton<IHostedService, CleanupScheduledService>();
            services.AddSingleton<IHostedService, CarparkHubUpdateScheduledService>();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                .AllowCredentials()
                                                             .AllowAnyMethod()
                                                              .AllowAnyHeader()));

            services.AddSignalR(options => { });

            services.AddMvc();


            //// Build the intermediate service provider
            var sp = services.BuildServiceProvider();

            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                return Bus.Factory.CreateUsingAzureServiceBus(cfg =>
                {
                    var connectionstring = Configuration.GetConnectionString("ServiceBusConnectionString");
                cfg.Host(connectionstring, sbc => { });
                cfg.ReceiveEndpoint("ApiProcessor", e => { e.Consumer(typeof(ApiConsumer), type => new ApiConsumer(sp.GetService<CarparkStatusManager>(), sp.GetService<ILogger<ApiConsumer>>())); });

            });
            })
            .As<IBusControl>()
            .As<IPublishEndpoint>()
            .SingleInstance();
            builder.Populate(services);
            container = builder.Build();
            
            return new AutofacServiceProvider(container);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime applicationLifetime)
        {
            app.UseCors("AllowAll");
            app.UseApplicationUrlMiddleware();
            app.UseApiKeyMiddleware();
            app.UseSignalR(options => options.MapHub<CarparkHub>("/broadcast"));

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseMvc();

            app.ConfigureServiceBusLifetime(applicationLifetime, container.Resolve<IBusControl>());



            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI V1");
            });

        }
    }
}
