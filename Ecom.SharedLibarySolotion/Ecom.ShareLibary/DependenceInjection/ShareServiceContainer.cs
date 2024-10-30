using Ecom.ShareLibary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.ShareLibary.DependenceInjection
{
    public static class ShareServiceContainer
    {
        public static IServiceCollection AddShareService<TContext>(this IServiceCollection services, IConfiguration config, string fileName) where TContext : DbContext
        {
            //Add dbContext
            services.AddDbContext<TContext>(option => option.UseSqlServer(config.GetConnectionString("EcomConnection"), sqlServerOption => sqlServerOption.EnableRetryOnFailure()));
            
            //Config logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services,config);
            return services;
        }
        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<GlobalException>();
            applicationBuilder.UseMiddleware<ListenToOnlyApiGateWay>();
            return applicationBuilder;
        }
    }
}
