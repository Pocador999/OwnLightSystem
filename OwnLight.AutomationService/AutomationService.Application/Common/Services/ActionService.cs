using System.Net.Http.Headers;
using System.Text;
using AutomationService.Application.Common.Services.Interfaces;
using AutomationService.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AutomationService.Application.Common.Services;

public class ActionService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    : IActionService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<bool> ControlDeviceAsync(
        Guid deviceId,
        RoutineActionType actionType,
        CancellationToken cancellationToken
    )
    {
        var requestUrl = $"http://localhost:5034/api/DeviceAction/control/{deviceId}";

        var token = _httpContextAccessor
            .HttpContext?.Request.Headers.Authorization.ToString()
            .Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedAccessException("Token JWT não encontrado.");

        var content = new StringContent(
            JsonConvert.SerializeObject(
                new { status = actionType == RoutineActionType.TurnOn ? 1 : 0 }
            ),
            Encoding.UTF8,
            "application/json"
        );

        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl) { Content = content };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        return response.IsSuccessStatusCode;
    }
}
