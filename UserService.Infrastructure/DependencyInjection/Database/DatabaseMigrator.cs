using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure.DependencyInjection
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using UserService.Infrastructure.Data;

    public class DatabaseMigrator
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Migrate()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var connectionString = configuration.GetConnectionString("DefaultConnection")!;

                    // Step 1: Check if the database exists, and create it if it doesn't
                    EnsureDatabaseExists(connectionString);

                    // Step 2: Apply EF Core migrations
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate(); // Apply any pending migrations
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DatabaseMigrator>>();
                    logger.LogError(ex, "An error occurred creating the database or applying migrations.");
                }

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate(); // Apply any pending migrations
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DatabaseMigrator>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
        }


        // Helper method to check if the database exists and create it if not
        private void EnsureDatabaseExists(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            // Connect to the master database to check if the target database exists
            var masterConnectionString = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master" // Use master database to check and create the target database
            }.ConnectionString;

            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                var checkDatabaseExistsCommand = connection.CreateCommand();
                checkDatabaseExistsCommand.CommandText = $@"
            IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{builder.InitialCatalog}')
            BEGIN
                CREATE DATABASE [{builder.InitialCatalog}]
            END";

                checkDatabaseExistsCommand.ExecuteNonQuery();
            }
        }
    }
}
