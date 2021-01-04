using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.BLL.Mappings;
using MoneyManager.BLL.Services.Entities;
using MoneyManager.DAL.Database;
using MoneyManager.DAL.Interfaces;
using AutoMapper;
using MoneyManager.WEB.Mappings;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace MoneyManager
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

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
            services.AddTransient<ITransactionTypeService, TransactionTypeService>();
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

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            //temporary solution
            //FIX: Use Docker volume
            //FIX: Services availability
            //FIX: Mirgrate constant to config
            var path = Path.Combine(Directory.GetCurrentDirectory(), "images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = new PathString("/images"),
            });
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //});


            //endpoints.MapControllerRoute(
            //  name: "default",
            //  pattern: "{controller}/{action=Index}/{id?}");

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
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
