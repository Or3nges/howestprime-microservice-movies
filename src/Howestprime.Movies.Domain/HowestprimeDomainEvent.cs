using Domaincrafters.Domain;

namespace Howestprime.Movies.Domain.Shared;

public abstract class HowestprimeDomainEvent(string eventName) : BaseDomainEvent(eventName, "movies")
{
}