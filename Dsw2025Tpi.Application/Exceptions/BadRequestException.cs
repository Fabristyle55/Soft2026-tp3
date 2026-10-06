namespace Dsw2025Ej15.Application.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string message) : base(message, 400) { }
}
