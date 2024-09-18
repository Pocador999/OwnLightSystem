namespace UserService.Application.Common.Messages;

public class Messages(string title, string message, string type, string statusCode)
{
    public string? Title { get; set; } = title;
    public string? Message { get; set; } = message;
    public string? Type { get; set; } = type;
    public string? StatusCode { get; set; } = statusCode;
    public Guid Id { get; set; } = Guid.NewGuid();

    public static Messages NotFound(string title, string message, string type, string statusCode) =>
        new(title, message, type, statusCode);
    
    public static Messages Success(string title, string message, string type, string statusCode) =>
        new(title, message, type, statusCode);

    public static Messages Error(string title, string message, string type, string statusCode) =>
        new(title, message, type, statusCode);
}
