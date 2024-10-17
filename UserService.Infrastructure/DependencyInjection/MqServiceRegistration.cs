using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Identity;
using UserService.Infrastructure.Messaging.RabbitMq;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.DependencyInjection
{
    public static class MqServiceRegistration
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQSettings = configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
            services.AddSingleton(rabbitMQSettings!);

            // Register the RabbitMQ consumer as a background service
            services.AddSingleton<RabbitMqUserPreferenceConsumer>(); // Register the consumer as a singleton
            services.AddHostedService<RabbitMqUserPreferenceConsumer>(); // Register it as a hosted service

            return services;
        }
    }
}
