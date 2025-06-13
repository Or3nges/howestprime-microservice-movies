using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Events;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Application.Movies.BookMovieEvent
{
    public class BookMovieEventHandler
    {
        private readonly IMovieEventRepository _movieEventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRoomRepository _roomRepository;

        public BookMovieEventHandler(
            IMovieEventRepository movieEventRepository,
            IUnitOfWork unitOfWork,
            IEventPublisher eventPublisher,
            IRoomRepository roomRepository)
        {
            _movieEventRepository = movieEventRepository;
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
            _roomRepository = roomRepository;
        }

        public async Task<BookMovieEventResult> HandleAsync(BookMovieEventCommand command)
        {
            if (command.StandardVisitors < 0 || command.DiscountVisitors < 0)
                throw new ArgumentException("Visitor counts must be non-negative.");
            if (command.StandardVisitors + command.DiscountVisitors <= 0)
                throw new ArgumentException("Total visitor count must be greater than 0.");

            var movieEvent = await _movieEventRepository.GetByIdWithBookingsAsync(command.MovieEventId);
            if (movieEvent == null)
                throw new InvalidOperationException("Movie event not found.");

            string roomName = command.RoomName ?? "";
            if (string.IsNullOrWhiteSpace(roomName))
            {
                var room = await _roomRepository.GetByIdAsync(movieEvent.RoomId);
                roomName = room?.Name ?? "Unknown Room";
            }

            var booking = movieEvent.BookEvent(command.StandardVisitors, command.DiscountVisitors, roomName);
            await _movieEventRepository.UpdateAsync(movieEvent);

            var bookingOpened = new BookingOpened(
                booking,
                movieEvent.MovieId,
                roomName,
                movieEvent.Time);
            await _eventPublisher.PublishAsync(bookingOpened);

            await _unitOfWork.CommitAsync();

            return new BookMovieEventResult { BookingId = booking.Id };
        }
    }
}
