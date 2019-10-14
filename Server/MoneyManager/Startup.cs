using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.BLL.Mappings;
using MoneyManager.BLL.Services.Entities;
using MoneyManager.DAL.Database;
using MoneyManager.DAL.Interfaces;
using AutoMapper;
using MoneyManager.WEB.Mappings;

namespace MoneyManager
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            if (_environment.IsDevelopment())
            {
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = @"..\..\Client\dist";
                });
            }

            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ITransactionsService, TransactionsService>();
            services.AddTransient<IFundService, FundService>();
            services.AddAutoMapper(typeof(DTOToEntityProfile), typeof(ViewToDTOProfile));
        }

        public void Configure(IApplicationBuilder app)
        {
            var clientUrl = _configuration.GetSection("Client").GetSection("Url").Value;
            app.UseCors(builder => builder.WithOrigins(clientUrl).AllowAnyMethod().AllowAnyHeader());

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
          

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            if (_environment.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = @"..\..\Client";
                    spa.UseReactDevelopmentServer(npmScript: "start");

                });
            }
        }
    }
}
