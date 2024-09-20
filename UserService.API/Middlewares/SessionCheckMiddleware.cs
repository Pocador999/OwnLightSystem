using UserService.Domain.Interfaces;

namespace UserService.API.Middlewares;

public class SessionCheckMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        var userSessionId = context.Session.GetString("UserId");

        if (!string.IsNullOrEmpty(userSessionId))
        {
            var userId = Guid.Parse(userSessionId);

            using var scope = serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var authRepository = scope.ServiceProvider.GetRequiredService<IAuthRepository>();

            var user = await userRepository.FindByIdAsync(userId);

            if (user != null && user.IsLogedIn)
            {
                var sessionTimeout = TimeSpan.FromMinutes(60);
                if (DateTime.UtcNow - user.LastLoginAt > sessionTimeout)
                {
                    await authRepository.LogoutAsync(userId);
                    context.Session.Clear();
                }
                else
                {
                    user.Login();
                    await userRepository.UpdateAsync(user);
                }
            }
        }

        await _next(context);
    }
}
