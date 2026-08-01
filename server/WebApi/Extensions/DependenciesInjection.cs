using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Minio;
using MoneyManager.Application.Extensions;
using MoneyManager.Infrastructure.Database;
using MoneyManager.WebApi.Mappings;
using System.Net.Http;
using System.Text;

namespace MoneyManager.WebApi.Extensions
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddMinioConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var fileStorageSection = configuration.GetSection("FileStorage");

            var endpoint = fileStorageSection.GetSection("Endpoint").Value;
            var useSsl = bool.Parse(fileStorageSection.GetSection("UseSsl").Value);

            var user = fileStorageSection.GetSection("User").Value;
            var password = fileStorageSection.GetSection("Password").Value;

            services.AddMinio(configureClient => configureClient
                .WithEndpoint(endpoint)
                .WithSSL(useSsl)
                .WithCredentials(user, password)
                .WithHttpClient(new HttpClient(new HttpClientHandler
                {
                    // TODO: fix self signed cert workaround
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                }))
                .Build());

            return services;
        }

        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var authSection = configuration.GetSection("Auth");

            var issuer = authSection.GetSection("Issuer").Value;
            var audience = authSection.GetSection("Audience").Value;
            var secret = authSection.GetSection("Secret").Value;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });

            return services;
        }

        public static IServiceCollection AddClientCors(this IServiceCollection services, IConfiguration configuration)
        {
            var clientUrl = configuration.GetSection("Client").GetSection("Url").Value;
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins(clientUrl)
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .AllowAnyHeader();
                    });
            });

            return services;
        }

        public static IServiceCollection AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnection = configuration.GetSection("DB").GetSection("ConnectionString").Value;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(dbConnection));

            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddApplicationProfile();
                cfg.AddProfile<ViewToDTOProfile>();
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
