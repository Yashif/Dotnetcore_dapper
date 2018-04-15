using System;
using System.Data;
using System.Data.SqlClient;
using ExpenseTrackerCoreBll;
using ExpenseTrackerCoreDal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
namespace ExpenseTrackerCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }
        private IDbConnection DbConnection;
        public void ConfigureServices(IServiceCollection services)
        {
         
            services.AddSingleton( (conn) => DbConnection);
            services.AddTransient<IRepository<Expense>, ExpenseRepository>();
            services.AddMvc();
            services.AddAutoMapper();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Auric API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                DbConnection =  new SqliteConnection($"Data Source={Environment.CurrentDirectory}_AuricDBSqlite.sqlite");
               
            }
            else
            {
                DbConnection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url:"/swagger/v1/swagger.json", name: "Auric API V1");
            });
        }
    }
}
