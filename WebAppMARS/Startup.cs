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
using DatabaseImplement.Implements;
using BusinessLogics;
using Contracts.BusinessLogics;
using Microsoft.OpenApi.Models;
namespace WebAppMARS
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestApi v1"));
            }
          
            app.UseHttpsRedirection();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
