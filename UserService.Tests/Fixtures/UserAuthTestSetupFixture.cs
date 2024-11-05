using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        public Mock<IHttpContextAccessor> HttpContextMock { get; private set; }

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

            // Add required dependencies for SignInManager
            services.AddSingleton<IOptions<IdentityOptions>, OptionsManager<IdentityOptions>>();
            services.AddSingleton<ILogger<SignInManager<User>>, Logger<SignInManager<User>>>();
            services.AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();

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

            // HttpContext mocks
            var httpContext = new DefaultHttpContext();
            HttpContextMock = new Mock<IHttpContextAccessor>();
            HttpContextMock.Setup(_ => _.HttpContext).Returns(httpContext);

            // Mock AuthenticationService for SignInManager
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            services.AddSingleton(authenticationServiceMock.Object);

            services.AddSingleton(UnitOfWorkMock.Object);
            services.AddSingleton(LoggerMock.Object);
            services.AddSingleton(HttpContextMock.Object);
        }

        public void Dispose()
        {
            if (ServiceProvider is IAsyncDisposable asyncDisposable)
            {
                asyncDisposable.DisposeAsync().GetAwaiter().GetResult();
            }
            else
            {
                ServiceProvider.Dispose();
            }
        }
    }
}
