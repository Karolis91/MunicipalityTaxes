namespace MunicipalityTaxes
{
    using System;
    using System.IO;
    using System.Reflection;
    using Controllers.ControllerAttributes;
    using DatabaseAgent;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Middleware;
    using Persistence;
    using Persistence.Extensions;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IDbContext, MunicipalityTaxContext>(opt => opt.UseSqlServer(Configuration.GetSection("MyLocalDB").Value));

            services.AddScoped<IMunicipalityTaxApplicationService, MunicipalityTaxApplicationService>();
            services.AddScoped<IMunicipalityTaxDatabaseAgent, MunicipalityTaxDatabaseAgent>();

            services.AddMvc(options => options.Filters.Add(typeof(ValidateModelStateAttribute)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("MunicipalityTaxV1", new Info { Title = "Municipality Taxes API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
           {
               app.UseDeveloperExceptionPage();
           }
           else
           {
               // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               app.UseHsts();
           }

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "municipalitytax/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("MunicipalityTaxV1/swagger.json", "Municipality Taxes V1");
                c.RoutePrefix = "municipalitytax/swagger";
            });

            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();
        }

        public static void MigrateDatabase(IWebHost host)
        {
            var scope = host.Services.CreateScope();
            using (scope)
            {
                var baseOptions = scope.ServiceProvider.GetRequiredService<DbContextOptions<MunicipalityTaxContext>>();
                var options = new DbContextOptionsBuilder<MunicipalityTaxContext>(baseOptions).UseSqlServer(Configuration.GetSection("MyLocalDB").Value);

                using (var db = new MunicipalityTaxContext(options.Options))
                {
                    db.MigrateDatabase();
                }
            }
        }
    }
}
