namespace AutomationService.Application.Common.Services.Interfaces;

public interface IAuthService
{
    public Task AuthenticateAsync(CancellationToken cancellationToken = default);
}
