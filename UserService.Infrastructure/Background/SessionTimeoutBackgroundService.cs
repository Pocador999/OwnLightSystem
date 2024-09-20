using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Background;

public class SessionTimeoutBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var authRepository = scope.ServiceProvider.GetRequiredService<IAuthRepository>();

            var loggedInUsers = await authRepository.GetAllLogedInUsersAsync();

            foreach (var user in loggedInUsers)
            {
                var sessionTimeout = TimeSpan.FromMinutes(60);
                if (DateTime.UtcNow - user.LastLoginAt > sessionTimeout)
                {
                    await authRepository.LogoutAsync(user.Id);
                }
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}
