using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure;
using UserService.Infrastructure.DependencyInjection;

namespace UserService.Tests.Fixtures
{
    public class UserAuthTestSetupFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }
        public Mock<IUserRepository> UserRepositoryMock { get; private set; }
        public Mock<ILogService> LoggerMock { get; private set; }

        public UserAuthTestSetupFixture()
        {
            var configuration = ConfigureJwtSettings();
            var services = ConfigureServices(configuration);
            ConfigureMocks(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        private static IConfiguration ConfigureJwtSettings()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:SecretKey"] = "r1djb8fMs1Tw0hKcVT2nC9mbJ6gigkN6",
                    ["Jwt:Issuer"] = "YourIssuer",
                    ["Jwt:Audience"] = "YourAudience"
                })
                .Build();
        }

        private ServiceCollection ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.ConnectToDatabase(configuration, options => options.UseInMemoryCache = true);
            services.AddIdentityAndJwtRegistration(configuration);
            services.AddInfrastructureServices();
            services.AddSingleton(configuration);
            services.AddLogging();
            return services;
        }

        private void ConfigureMocks(ServiceCollection services)
        {
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            UserRepositoryMock = new Mock<IUserRepository>();
            LoggerMock = new Mock<ILogService>();

            // UserRepository Mock Setup
            UserRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            UserRepositoryMock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string username) => new User("testuser@example.com", "Test User", username));

            // UnitOfWork Mock Setup
            UnitOfWorkMock.Setup(uow => uow.Users).Returns(UserRepositoryMock.Object);
            UnitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            services.AddSingleton(UnitOfWorkMock.Object);
            services.AddSingleton(LoggerMock.Object);
        }

        public void Dispose() => ServiceProvider?.Dispose();
    }
}
