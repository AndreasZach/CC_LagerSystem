using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.Interfaces;
using StorageApi.Models;

namespace StorageApi
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
            services.AddMvc().AddNewtonsoftJson();
            services.AddDbContext<StorageItemContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("StorageContext")));
            services.AddScoped<IDataRepository, DataRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Storage}/{action=Index}/{itemName?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<StorageItemContext>()
                    ?.Database
                    .Migrate();
                if (!serviceScope.ServiceProvider.GetService<StorageItemContext>().StorageItems.Any())
                {
                    serviceScope.ServiceProvider
                        .GetService<StorageItemContext>()
                        ?.AddRange(new List<StorageItem>
                        {
                            new StorageItem("Ost") {ItemAmount = 0},
                            new StorageItem("Tomatsås") {ItemAmount = 0},
                            new StorageItem("Skinka") {ItemAmount = 0},
                            new StorageItem("Ananas") {ItemAmount = 0},
                            new StorageItem("Kebab") {ItemAmount = 0},
                            new StorageItem("Champinjoner") {ItemAmount = 0},
                            new StorageItem("Lök") {ItemAmount = 0},
                            new StorageItem("Feferoni") {ItemAmount = 0},
                            new StorageItem("Isbergssallad") {ItemAmount = 0},
                            new StorageItem("Tomat") {ItemAmount = 0},
                            new StorageItem("Kebabsås") {ItemAmount = 0},
                            new StorageItem("Räkor") {ItemAmount = 0},
                            new StorageItem("Musslor") {ItemAmount = 0},
                            new StorageItem("Kronärtskocka") {ItemAmount = 0},
                            new StorageItem("Coca cola") {ItemAmount = 0},
                            new StorageItem("Fanta") {ItemAmount = 0},
                            new StorageItem("Sprite") {ItemAmount = 0}
                        });
                }
                serviceScope.ServiceProvider
                    .GetService<StorageItemContext>()
                    ?.SaveChanges();
            }
        }
    }
}
