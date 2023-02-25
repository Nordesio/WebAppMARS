using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.StorageContracts;
using Contracts.ViewModels;
using DatabaseImplement;
using BusinessLogics;
using Contracts.BusinessLogics;
using Microsoft.EntityFrameworkCore;
using DatabaseImplement.Implements;
namespace WebApp
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
            
            services.AddTransient<IBuyerStorage, BuyerStorage>();
            services.AddTransient<IProductStorage, ProductStorage>();
            services.AddTransient<ISalesPointStorage, SalesPointStorage>();
            services.AddTransient<ISaleStorage, SaleStorage>();
            
            services.AddTransient<IBuyerLogic, BuyerLogic>();
            services.AddTransient<IProductLogic, ProductLogic>();
            services.AddTransient<ISalesPointLogic, SalesPointLogic>();
            services.AddTransient<ISaleLogic, SaleLogic>();
            
            services.AddControllers();
            services.AddControllersWithViews();
            //services.AddScoped<SalesDatabase, SalesDatabase>();
           // services.AddDbContext<SalesDatabase>(item => item.UseSqlServer(Configuration.GetConnectionString("DefaultDatabase")));
         }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=BuyerAndSales}/{action=Enter}/{id?}");
            });
        }
    }
}
