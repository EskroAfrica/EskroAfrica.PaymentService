using EskroAfrica.PaymentService.Application;
using EskroAfrica.PaymentService.Application.Identity;
using EskroAfrica.PaymentService.Application.Implementations;
using EskroAfrica.PaymentService.Application.Interfaces;
using EskroAfrica.PaymentService.Common.Models;
using EskroAfrica.PaymentService.Infrastructure.Data;
using EskroAfrica.PaymentService.Infrastructure.Implementations;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Reflection;

namespace EskroAfrica.PaymentService.API
{
    public static class StartupHelper
    {
        public static void AddConfigurations(this IServiceCollection services, IConfiguration config)
        {
            // Bind AppSettings
            services.Configure<AppSettings>(config.GetSection(nameof(AppSettings)));
            services.AddScoped(config => config.GetRequiredService<IOptions<AppSettings>>().Value);

            var appSettings = services.BuildServiceProvider().GetService<AppSettings>();

            // AddDbContext
            services.AddDbContext<PaymentServiceDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            // Add Authentication
            JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = appSettings.IdentitySettings.Authority;
                    options.Audience = appSettings.IdentitySettings.Audience;
                    options.TokenValidationParameters.ValidateIssuer = appSettings.IdentitySettings.ValidateIssuer;
                    options.TokenValidationParameters.ValidateAudience = appSettings.IdentitySettings.ValidateAudience;
                    options.TokenValidationParameters.NameClaimType = "given_name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanAccessApp", AuthorizationPolicies.CanAccessApp());
                options.AddPolicy("CanAccessBackOffice", AuthorizationPolicies.CanAccessBackOffice());
            });

            // Add Hangfire
            services.AddHangfire(options => options.UseSqlServerStorage(config.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            // Add Automapper
            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            // Add HttpCLientFactory
            services.AddHttpClient();

            // Add Kafka
            services.AddScoped<IKafkaProducerService, KafkaProducerService>();
            //services.AddHostedService<KafkaConsumerService>();

            // Add Services

            services.AddScoped<IHttpClientService, HttpClientService>();
            services.AddScoped<IPaystackService, PaystackService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
