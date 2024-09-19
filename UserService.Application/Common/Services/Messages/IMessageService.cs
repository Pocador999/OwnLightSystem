namespace UserService.Application.Common.Services.Messages;

public interface IMessageService
{
    Message CreateValidationMessage(IEnumerable<string> errors);
    Message CreateNotAuthorizedMessage(string message);
    Message CreateBadRequestMessage(string message);
    Message CreateConflictMessage(string message);
    Message CreateNotFoundMessage(string message);
    Message CreateSuccessMessage(string message);
}
