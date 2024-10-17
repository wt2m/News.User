using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqUserPreferenceConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider; // Inject IServiceProvider
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;
        private readonly RabbitMQSettings _settings;

        public RabbitMqUserPreferenceConsumer(IServiceProvider serviceProvider, RabbitMQSettings settings)
        {
            _serviceProvider = serviceProvider; // Use service provider to resolve scoped services
            _settings = settings;

            var factory = new ConnectionFactory() { HostName = _settings.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var headers = ea.BasicProperties.Headers;

                if (headers.TryGetValue("Authorization", out object tokenObj))
                {
                    var token = Encoding.UTF8.GetString((byte[])tokenObj).Replace("Bearer ", "");

                    // Create a scope to resolve both IUserService and ITokenService
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                        var userId = tokenService.GetUserIdByToken(token);

                        if (userId != Guid.Empty)
                        {
                            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                            var preference = message.Trim();
                            await userService.UpdateUserPreferencesAsync(userId, preference);
                        }
                    }
                }
            };

            _channel.BasicConsume(queue: "user_preference_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
