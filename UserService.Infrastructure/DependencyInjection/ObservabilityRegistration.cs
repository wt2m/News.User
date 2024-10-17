using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Observability.ElasticSearch;

namespace UserService.Infrastructure.DependencyInjection
{
    public static class ObservabilityRegistration
    {
        public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new Uri(configuration["ElasticSearch:Uri"]!));
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            services.AddScoped<IObservationService, ElasticSearchUserLogs>();

            return services;
        }
    }
}
