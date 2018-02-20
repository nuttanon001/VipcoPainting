using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VipcoPainting.Models;
using VipcoPainting.Services.Classes;
using VipcoPainting.Services.Interfaces;

namespace VipcoPainting
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
			services.AddMvc();
            // this is in package Microsoft.AspNetCore.NodeServices
            services.AddNodeServices();
            // Add AutoMapper
            AutoMapper.Mapper.Reset();
            services.AddAutoMapper(typeof(Startup));
            // Add DbContext.
            // Change AddDbContextPool if EF Core 2.1
            services.AddDbContext<PaintingContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("PaintingConnection")))
                    .AddDbContext<MachineContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("MachineConnection")));

            // Add Repositoy
            //services.AddTransient(typeof(IRepositoryMachine<>), typeof(RepositoryMachine<>));
            //services.AddTransient(typeof(IRepositoryPainting<>), typeof(RepositoryPainting<>));
            services.AddTransient(typeof(IRepositoryMachine<>), typeof(RepositoryMachine<>))
                    .AddTransient(typeof(IRepositoryPainting<>), typeof(RepositoryPainting<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            String pathBase = Configuration.GetSection("Hosting")["PathBase"];
            if (String.IsNullOrEmpty(pathBase) == false)
                app.UsePathBase(pathBase);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
