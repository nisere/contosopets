using ContosoPets.DataAccess.Data;
using ContosoPets.DataAccess.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace ContosoPets.Api
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
        //    services.AddDbContext<ContosoPetsContext>(options =>
        //options.UseInMemoryDatabase("ContosoPets"));

            services.AddScoped<OrderService>();

            var builder = new SqlConnectionStringBuilder(
    Configuration.GetConnectionString("ContosoPets"));

            //IConfigurationSection contosoPetsCredentials =
            //    Configuration.GetSection("ContosoPetsCredentials");

            //builder.UserID = contosoPetsCredentials["UserId"];
            //builder.Password = contosoPetsCredentials["Password"];

            services.AddDbContext<ContosoPetsContext>(options =>
                options.UseSqlServer(builder.ConnectionString)
                       .EnableSensitiveDataLogging(Configuration.GetValue<bool>("Logging:EnableSqlParameterLogging")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ContosoPetsContext context)
        {
            context.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

			SeedData.Initialize(context);
        }
    }
}
