namespace Howestprime.Movies.Domain.Shared.Exceptions;

public sealed class NotFoundException(string message) : Exception(message)
{
}