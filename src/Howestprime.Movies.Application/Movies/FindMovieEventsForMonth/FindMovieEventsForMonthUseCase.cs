using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Application.Movies.FindMovieEventsForMonth
{
    public class FindMovieEventsForMonthUseCase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieEventRepository _movieEventRepository;
        private readonly IRoomRepository _roomRepository;

        public FindMovieEventsForMonthUseCase(
            IMovieRepository movieRepository,
            IMovieEventRepository movieEventRepository,
            IRoomRepository roomRepository)
        {
            _movieRepository = movieRepository;
            _movieEventRepository = movieEventRepository;
            _roomRepository = roomRepository;
        }

        public async Task<List<MovieEventData>> ExecuteAsync(FindMovieEventsForMonthQuery query)
        {
            if (query.Month < 1 || query.Month > 12 || query.Year < 1)
                return new List<MovieEventData>();

            var start = new DateTime(query.Year, query.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddMonths(1).AddTicks(-1);
            var events = await _movieEventRepository.GetEventsInRangeAsync(start, end);
            var result = new List<MovieEventData>();
            foreach (var ev in events)
            {
                var movie = await _movieRepository.GetByIdAsync(ev.MovieId);
                var room = await _roomRepository.GetByIdAsync(ev.RoomId);
                if (movie == null || room == null) continue;
                result.Add(new MovieEventData
                {
                    Id = ev.Id,
                    Date = DateOnly.FromDateTime(ev.Date),
                    Time = TimeOnly.FromTimeSpan(ev.Time),
                    Room = new RoomData { Id = room.Id, Name = room.Name, Capacity = room.Capacity },
                    Capacity = ev.Capacity,
                    Visitors = 0
                });
            }
            return result;
        }
    }
}
