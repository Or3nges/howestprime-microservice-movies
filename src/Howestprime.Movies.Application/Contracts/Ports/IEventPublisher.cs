using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Events;
using Howestprime.Movies.Domain.Booking;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IEventPublisher
    {
        Task PublishAsync(BookingOpened bookingOpenedEvent);
    }
}