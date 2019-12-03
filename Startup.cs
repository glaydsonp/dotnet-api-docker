using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalog.Data;
using ProductCatalog.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;

namespace ProductCatalog
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                MvcOptions => MvcOptions.EnableEndpointRouting = false
            );
            services.AddResponseCompression();
            services.AddScoped<StoreDataContext, StoreDataContext>();
            services.AddTransient<ProductRepository, ProductRepository>();
            services.AddTransient<CategoryRepository, CategoryRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMvc();
            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {

                // endpoints.MapRazorPages();
                // endpoints.MapFallbackToPage("_Host");
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                // });
            });
        }
    }
}
