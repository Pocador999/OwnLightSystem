namespace UserService.Application.Common.Services.Messages;

public class Message(string title, string content, string type, string statusCode)
{
    public string? Title { get; set; } = title;
    public string? Content { get; set; } = content;
    public string? Type { get; set; } = type;
    public string? StatusCode { get; set; } = statusCode;
    public Guid Id { get; set; } = Guid.NewGuid();

    public static Message NotFound(string title, string message, string type, string statusCode) =>
        new(title, message, type, statusCode);
    
    public static Message Success(string title, string message, string type, string statusCode) =>
        new(title, message, type, statusCode);

    public static Message Error(string title, string message, string type, string statusCode) =>
        new(title, message, type, statusCode);
}
