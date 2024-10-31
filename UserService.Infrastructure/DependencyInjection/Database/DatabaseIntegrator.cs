using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.DependencyInjection
{
    public static class DatabaseIntegrator
    {
        public static IServiceCollection ConnectToDatabase(this IServiceCollection services, IConfiguration configuration, Action<DatabaseOptions> setupAction)
        {
            // Add the ApplicationDbContext with SQL 


            // Create a new DatabaseOptions instance
            var options = new DatabaseOptions();
            setupAction(options);

            // Register ApplicationDbContext with the appropriate database based on DatabaseOptions
            if (options.UseInMemoryCache)
            {
                // Use InMemory database for testing or lightweight scenarios
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));
            }
            else if (options.UseSQLServer)
            {
                // Use SQL Server database
                services.AddDbContext<ApplicationDbContext>(opt =>
                    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                throw new InvalidOperationException("No database option selected. Please configure DatabaseOptions.");
            }

            return services;
        }
    }
}
